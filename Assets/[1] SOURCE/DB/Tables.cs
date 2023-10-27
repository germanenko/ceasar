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
			ConstantSingleton.Instance.DbManager.CreateTable<SavesAndDrafts>();
            //Debug.Log("end init");
            //CreateTableText();

            string sqlDrafts = $"SELECT * FROM SavesAndDrafts WHERE Draft = 1";
            string sqlSave = $"SELECT * FROM SavesAndDrafts WHERE Reference != 0";

            List<SavesAndDrafts> draftDate = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sqlDrafts);
            List<SavesAndDrafts> saveDate = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sqlSave);

            if (!draftDate.IsNullOrEmpty() && DateTime.Today.Day - draftDate[0].Date.Day > 7)
            {
                Debug.Log("delete draft");
                DropDraft();
            }

            for (int i = 0; i < saveDate.Count; i++)
            {
                if(DateTime.Today.Day - saveDate[i].Date.Day > 7)
                {
                    string dropSaveTask = $"DELETE FROM Tasks WHERE ID = {saveDate[i].TaskID}";
                    string dropSave = $"DELETE FROM SavesAndDrafts WHERE ID = {saveDate[i].ID}";
                    ConstantSingleton.Instance.DbManager.Execute(dropSaveTask);
                    ConstantSingleton.Instance.DbManager.Execute(dropSave);
                }
            }
        }



		public void AddTask(string name, string color)
		{

			var taskName = ConstantSingleton.Instance.TaskFormManager.Task.Name;
			var taskType = ((TypeOfTasks)ConstantSingleton.Instance.TaskFormManager.Task.Type).ToString();
			var taskColor = ConstantSingleton.Instance.TaskFormManager.Task.Color;

			ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Load) VALUES (?, ?, ?, ?)",
                name == null ? "" : name,
				"",
                color == null ? "ffffffff" : color, 
                true);

            SetPriority();
        }



        public void AddDraft(string name, string color)
        {
            ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Load) VALUES (?, ?, ?, ?)",
                name == null ? "" : name,
                "",
                color == null ? "ffffffff" : color,
                true);

            var task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks");

            ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO SavesAndDrafts (TaskID, Reference, Draft, Date) VALUES (?, ?, ?, ?)",
                task[task.Count - 1].ID,
                0,
                true,
                DateTime.Today);

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
            string sql = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 0";
            List<SavesAndDrafts> saves = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sql);

            string sqlArchive = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 1";
            List<SavesAndDrafts> archive = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sqlArchive);

            if (saves.Count > 0)
            {
                List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks WHERE ID = {saves[0].TaskID}");

                if (task[0].Name != name || task[0].Color != color)
                {
                    AddSaveTask(task[0].Name, task[0].Color, id);
                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                }
                else
                {
                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                    DeleteTask(saves[0].TaskID);
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

            if (archive.Count > 0)
            {
                List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks WHERE ID = {archive[0].TaskID}");

                if (task[0].Name == name && task[0].Color == color)
                {
                    DeleteTask(archive[0].TaskID);
                    ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                }
            }
        }



        public void EditDraft(string name, string color)
        {
            string sql = $"SELECT * FROM SavesAndDrafts WHERE Reference = 0 AND Draft = 1";
            List<SavesAndDrafts> draft = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sql);

            ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, draft[0].TaskID);
        }



        public void AddSaveTask(string name, string color, int id = 0)
        {
            string checkSave = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 0";
            List<SavesAndDrafts> saves = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(checkSave);

            if (saves.IsNullOrEmpty())
            {
                ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Load) VALUES (?, ?, ?, ?)",
                name == null ? "" : name,
                "",
                color == null ? "ffffffff" : color,
                false);

                var task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks");

                ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO SavesAndDrafts (TaskID, Reference, Draft, Date) VALUES (?, ?, ?, ?)",
                task[task.Count - 1].ID,
                id,
                false,
                DateTime.Today);
            }
            else
            {
                ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                ConstantSingleton.Instance.DbManager.Execute("UPDATE SavesAndDrafts SET Date = ? WHERE TaskID = ?", DateTime.Today, saves[0].TaskID);
            }
        }



        public void AddArchiveTask(string name, string color, int id = 0)
        {
            string checkArchive = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 1";
            List<SavesAndDrafts> archives = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(checkArchive);

            string sql = $"SELECT * FROM Tasks WHERE ID = {id}";
            List<Tasks> tasks = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            if (archives.IsNullOrEmpty())
            {
                if (tasks[0].Name != name || tasks[0].Color != color)
                {
                    ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO Tasks (Name, Type, Color, Load) VALUES (?, ?, ?, ?)",
                        name == null ? "" : name,
                        "",
                        color == null ? "ffffffff" : color,
                        false);

                    var task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks");

                    ConstantSingleton.Instance.DbManager.Execute($"INSERT INTO SavesAndDrafts (TaskID, Reference, Draft, Date) VALUES (?, ?, ?, ?)",
                        task[task.Count - 1].ID,
                        id,
                        true,
                        DateTime.Today);
                }  
            }
            else
            {
                ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Name = ?, Color = ? WHERE ID = ?", name, color, id);
                ConstantSingleton.Instance.DbManager.Execute("UPDATE SavesAndDrafts SET Date = ? WHERE TaskID = ?", DateTime.Today, archives[0].TaskID);
            }
        }



        public Tasks GetSaveTask(int id)
        {
            string sql = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 0";

            List<SavesAndDrafts> save = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sql);

            if (save.Count > 0)
            {
                string taskSql = $"SELECT * FROM Tasks WHERE ID = {save[0].TaskID}";
                List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(taskSql);

                return task[0];
            }
            else
            {
                return null;
            }
        }



        public Tasks GetArchiveTask(int id)
        {
            string sql = $"SELECT * FROM SavesAndDrafts WHERE Reference = {id} AND Draft = 1";

            List<SavesAndDrafts> archive = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(sql);

            if (archive.Count > 0)
            {
                string taskSql = $"SELECT * FROM Tasks WHERE ID = {archive[0].TaskID}";
                List<Tasks> task = ConstantSingleton.Instance.DbManager.Query<Tasks>(taskSql);

                return task[0]; 
            }
            else
            {
                return null;
            }
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
            ConstantSingleton.Instance.DbManager.Execute("UPDATE SavesAndDrafts SET Draft = 0 WHERE TaskID = ?", id);
            ConstantSingleton.Instance.DbManager.Execute("UPDATE Tasks SET Load = 1 WHERE ID = ?", id);
        }



        public void DeleteTask(int id)
        {
            string deleteSql = $"DELETE FROM Tasks WHERE ID = {id}";
            string deletePriority = $"DELETE FROM Priority WHERE TaskID = {id}";
            string deleteSaves = $"DELETE FROM SavesAndDrafts WHERE TaskID = {id}";
            string updateAI = $"UPDATE sqlite_sequence SET seq = seq - 1 WHERE name IN ('Tasks', 'Priority')";

            try
            {
                ConstantSingleton.Instance.DbManager.Execute(deleteSql);
                ConstantSingleton.Instance.DbManager.Execute(deletePriority);
                ConstantSingleton.Instance.DbManager.Execute(deleteSaves);
                ConstantSingleton.Instance.DbManager.Execute(updateAI);
            }
            catch (Exception)
            {
                Debug.LogError("������ �������� ������");
            }
        }



        public void DropTable()
		{

			string sql;
			sql = "DROP TABLE \"Tasks\"";

            string sqlPriority;
            sqlPriority = "DROP TABLE \"Priority\"";

            string sqlSaves;
            sqlSaves = "DROP TABLE \"SavesAndDrafts\"";

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

            draft = $"SELECT * FROM SavesAndDrafts WHERE Draft = 1 AND Reference = 0";
            List<SavesAndDrafts> taskList = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(draft);

            dropDraft = "DELETE FROM Tasks WHERE Draft = 1 AND Reference = 0";

            dropPriorityDraft = $"DELETE FROM Priority WHERE TaskID = {taskList[0].TaskID}";

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
