using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System;
using Germanenko.Framework;
using System.Threading.Tasks;
using System.Drawing;
using DTT.Utils.Extensions;

namespace Germanenko.Source
{

	public class Tables
	{

		public void Init()
		{
			//Debug.Log("init "  + ConstantSingleton.Instance.DbManager);
			ConstantSingleton.Instance.DbManager.CreateTable<Tasks>();
			ConstantSingleton.Instance.DbManager.CreateTable<Priority>();
            //Debug.Log("end init");
            //CreateTableText();

            string sqlDrafts = $"SELECT * FROM Tasks WHERE Draft = 1";
            string sqlArchive = $"SELECT * FROM Tasks WHERE Reference != 0";

            List<Tasks> draftDate = ConstantSingleton.Instance.DbManager.Query<Tasks>(sqlDrafts);
            List<Tasks> archiveDate = ConstantSingleton.Instance.DbManager.Query<Tasks>(sqlArchive);

            if (!draftDate.IsNullOrEmpty() && DateTime.Today.Day - draftDate[0].Date.Day > 7)
            {
                Debug.Log("delete draft");
                DropDraft();
            }

            for (int i = 0; i < archiveDate.Count; i++)
            {
                if(DateTime.Today.Day - archiveDate[i].Date.Day > 7)
                {
                    string dropSave = $"DELETE FROM Tasks WHERE ID = {archiveDate[i].ID}";
                    ConstantSingleton.Instance.DbManager.Execute(dropSave);
                }
            }
        }



		public void AddTask(string name, string color)
		{

			var taskName = ConstantSingleton.Instance.TaskFormManager.Task.Name;
			var taskType = ((TypeOfTasks)ConstantSingleton.Instance.TaskFormManager.Task.Type).ToString();
			var taskColor = ConstantSingleton.Instance.TaskFormManager.Task.Color;

			ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Date) VALUES (?, ?, ?, ?)",
                name == null ? "" : name,
				"", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
                color == null ? "ffffffff" : color, DateTime.Today);

            SetPriority();
        }



        public void AddDraft(string name, string color)
        {
            ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Draft, Date) VALUES (?, ?, ?, ?, ?)",
                name == null ? "" : name,
                "", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
                color == null ? "ffffffff" : color, true, DateTime.Today);

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
            string sql = $"SELECT * FROM Tasks WHERE Reference = {id}";
            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            if(task.Count > 0)
            {
                if (task[0].Name != name || task[0].Color != color)
                {
                    AddSaveTask(task[0].Name, task[0].Color, id);

                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                }
                else
                {
                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                    DeleteTask(task[0].ID);
                }
            }
            else
            {
                string mainSql = $"SELECT * FROM Tasks WHERE ID = {id}";
                List<Tasks> mainTask = ConstantSingleton.Instance.DbManager.Query<Tasks>(mainSql);

                if (mainTask[0].Name != name || mainTask[0].Color != color)
                {
                    AddSaveTask(mainTask[0].Name, mainTask[0].Color, id);

                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                }
            }
        }



        public void AddSaveTask(string name, string color, int id = 0)
        {
            string checkSave = $"SELECT * FROM Tasks WHERE Reference = {id}";

            List<Tasks> saves = ConstantSingleton.Instance.DbManager.Query<Tasks>(checkSave);

            if (saves.IsNullOrEmpty())
            {
                ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Reference, Name, Type, Color, Date) VALUES (?, ?, ?, ?, ?)",
                id,
                name == null ? "" : name,
                "",
                color == null ? "ffffffff" : color, DateTime.Today);
            }
            else
            {
                ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ?, Date = ? WHERE Reference = ?", name, color, DateTime.Today, id);
            }
        }



        public void AddArchiveTask(string name, string color, int id = 0)
        {
            string checkSave = $"SELECT * FROM Tasks WHERE Reference = {id} AND Draft = 1";

            List<Tasks> saves = ConstantSingleton.Instance.DbManager.Query<Tasks>(checkSave);

            if (saves.IsNullOrEmpty())
            {
                ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Reference, Name, Type, Color, Date, Draft) VALUES (?, ?, ?, ?, ?, ?)",
                id,
                name == null ? "" : name,
                "",
                color == null ? "ffffffff" : color,
                DateTime.Today,
                1);
            }
            else
            {
                ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ?, Date = ? WHERE Reference = ? AND Draft = 1", name, color, DateTime.Today, id);
            }
        }



        public Tasks GetSaveTask(int id)
        {
            string sql = $"SELECT * FROM Tasks WHERE Reference = {id}";

            List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            if (task.Count > 0)
                return task[0];
            else
                return null;
        }



        public Tasks GetArchiveTask(int id)
        {
            string sql = $"SELECT * FROM Tasks WHERE Reference = {id} AND Draft = 1";

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



        public void DraftToTask(int id)
        {
            ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Draft = 0 WHERE ID = ?", id);
        }



        public void DeleteTask(int id)
        {
            string deleteSql = $"DELETE FROM Tasks WHERE ID = {id}";
            string updateAI = $"UPDATE sqlite_sequence SET seq = seq - 1 WHERE name IN ('Tasks', 'Priority')";

            try
            {
                ConstantSingleton.Instance.DbManager.Execute(deleteSql);
                ConstantSingleton.Instance.DbManager.Execute(updateAI);
            }
            catch (Exception)
            {
                Debug.LogError("Ошибка удаления задачи");
            }
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



        public void DropDraft()
        {
            string dropDraft;
            string dropPriorityDraft;
            string draft;
            string updateAI;

            updateAI = $"UPDATE sqlite_sequence SET seq = seq - 1 WHERE name IN ('Tasks', 'Priority')";

            draft = $"SELECT * FROM Tasks WHERE Draft = 1";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(draft);

            dropDraft = "DELETE FROM Tasks WHERE Draft = 1";
            dropPriorityDraft = $"DELETE FROM Priority WHERE TaskID = {taskList[0].ID}";

            try
            {
                ConstantSingleton.Instance.DbManager.Execute(dropDraft);
                ConstantSingleton.Instance.DbManager.Execute(dropPriorityDraft);
                ConstantSingleton.Instance.DbManager.Execute(updateAI);

            }
            catch (Exception)
            {
            }

        }
    }

}
