using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System;
using Germanenko.Framework;
using System.Threading.Tasks;
using System.Drawing;

namespace Germanenko.Source
{

	public class Tables
	{

		public void Init()
		{
			//Debug.Log("init "  + ConstantSingleton.Instance.DbManager);
			ConstantSingleton.Instance.DbManager.CreateTable<Tasks>();
			ConstantSingleton.Instance.DbManager.CreateTable<TaskSave>();
			ConstantSingleton.Instance.DbManager.CreateTable<Priority>();
		    //Debug.Log("end init");
			//CreateTableText();
		}



		public void AddTask(string name, string color)
		{

			var taskName = ConstantSingleton.Instance.TaskFormManager.Task.Name;
			var taskType = ((TypeOfTasks)ConstantSingleton.Instance.TaskFormManager.Task.Type).ToString();
			var taskColor = ConstantSingleton.Instance.TaskFormManager.Task.Color;

			ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color) VALUES (?, ?, ?)",
                name == null ? "" : name,
				"", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
                color == null ? "ffffffff" : color);

            SetPriority();

            AddSaveTask(name, color, false);
        }



        public void AddDraft(string name, string color)
        {
            ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Draft) VALUES (?, ?, ?, ?)",
                name == null ? "" : name,
                "", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
                color == null ? "ffffffff" : color, true);

            SetPriority();

        }



        private void SetPriority()
        {
            string sql = $"SELECT * FROM Tasks";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            Tasks lastTask = taskList[taskList.Count - 1];

            ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Priority (TaskID) VALUES (?)",
                lastTask.ID);
        }



        public void UpdatePriority()
        {
            for (int i = 0; i < Toolbox.Get<ListOfTasks>().Tasks.Count; i++)
            {
                int taskID = Toolbox.Get<ListOfTasks>().Tasks[i].ID;
                int taskSiblingIndex = Toolbox.Get<ListOfTasks>().Tasks[i].transform.GetSiblingIndex() + 1;

                Toolbox.Get<ListOfTasks>().Tasks[i].SetPriority(taskSiblingIndex);

                ConstantSingleton.Instance.DbManager.Execute("UPDATE Priority SET TaskID = ? WHERE PriorityValue = ?", taskID, taskSiblingIndex);

                Debug.Log($"updated: ID - {taskID}, Priority {taskSiblingIndex}");
            }
            Debug.Log("=========================================");

            //Toolbox.Get<ListOfTasks>().ReloadList();
        }



        public void EditTask(string name, string color, int id)
		{
            ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);

            AddSaveTask(name, color, true, id);
        }



        public void AddSaveTask(string name, string color, bool update, int id = 0)
        {
            string sql = $"SELECT * FROM Tasks";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            if (update)
            {
                ConstantSingleton.Instance.DbManager.Execute("UPDATE TaskSave SET Name = ?, Color = ? WHERE TaskID = ?", name, color, id);
            }
            else
            {
                Tasks lastTask = taskList[taskList.Count - 1];

                ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO TaskSave (TaskID, Name, Type, Color) VALUES (?, ?, ?, ?)",
                    lastTask.ID,
                    name == null ? "" : name,
                    "", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
                    color == null ? "ffffffff" : color);
            }
        }



        public Tasks GetSaveTask(int id)
        {
            string sql = $"SELECT * FROM TaskSave WHERE TaskID = {id}";

            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            if (task.Count > 0)
                return task[0];
            else
                return null;
        }



        public void CreateTableText()
		{
			string sql;

			ConstantSingleton.Instance.DbManager.BeginTransaction();

			// Create the table
			sql = "CREATE TABLE \"Tasks\" " +
					"(\"ID\" INTEGER PRIMARY KEY NOT NULL, " +
					"\"Name\" varchar(60) NOT NULL)";
			ConstantSingleton.Instance.DbManager.Execute(sql);

			//sql = "CREATE INDEX \"TaskBase\" on \"TaskBase\"(\"ID\")";

			ConstantSingleton.Instance.DbManager.Commit();

		}



		public void DropTable()
		{

			string sql;
			sql = "DROP TABLE \"Tasks\"";

            string sqlPriority;
            sqlPriority = "DROP TABLE \"Priority\"";

            string sqlSaves;
            sqlSaves = "DROP TABLE \"TaskSave\"";

            try
            {
				ConstantSingleton.Instance.DbManager.Execute(sql);
				ConstantSingleton.Instance.DbManager.Execute(sqlPriority);
				ConstantSingleton.Instance.DbManager.Execute(sqlSaves);
			}
			catch (Exception)
            {
            }

		}



        public void DraftToTask(int id)
        {
            ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Draft = 0 WHERE ID = ?", id);
        }



        public void DropDraft()
        {
            string dropDraft;
            string dropPriorityDraft;
            string draft;

            draft = $"SELECT * FROM Tasks WHERE Draft = 1";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(draft);

            dropDraft = "DELETE FROM Tasks WHERE Draft = 1";
            dropPriorityDraft = $"DELETE FROM Priority WHERE TaskID = {taskList[0].ID}";

            try
            {
                ConstantSingleton.Instance.DbManager.Execute(dropDraft);
                ConstantSingleton.Instance.DbManager.Execute(dropPriorityDraft);
            }
            catch (Exception)
            {
            }

        }
    }

}
