using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System;
using Germanenko.Framework;



namespace Germanenko.Source
{

	public class Tables
	{

		public void Init()
		{
			Debug.Log("inint "  + ConstantSingleton.Instance.DbManager);
			ConstantSingleton.Instance.DbManager.CreateTable<Tasks>();
			Debug.Log("end inint");
			//CreateTableText();
		}



		public void AddTask()
		{

			var taskName = ConstantSingleton.Instance.TaskFormManager.Task.Name;
			var taskType = ((TypeOfTasks)ConstantSingleton.Instance.TaskFormManager.Task.Type).ToString();
			var taskColor = ConstantSingleton.Instance.TaskFormManager.Task.Color;

			ConstantSingleton.Instance.DbManager.Execute("INSERT INTO Tasks (Name, Type, Color) VALUES (?, ?, ?)",
				taskName == null ? "" : taskName,
				"", //ConstantSingleton.Instance.TaskFormManager.Task.Type.ToString(),
				taskColor == null ? "ffffffff" : taskColor);

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

            try
            {
				ConstantSingleton.Instance.DbManager.Execute(sql);
			}
			catch (Exception)
            {
            }

		}

	}

}
