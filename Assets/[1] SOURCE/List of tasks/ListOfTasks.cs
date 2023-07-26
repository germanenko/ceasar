using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System;
using Germanenko.Framework;



namespace Germanenko.Source
{

	public class ListOfTasks : IReceive<SignalReloadList>
	{

		private List<ItemOfList> _tasks = new List<ItemOfList>();



		public ListOfTasks()
		{
			Signals.Add(this);
		}



		public void HandleSignal(SignalReloadList arg)
        {
			ReloadList();
        }



        public void ReloadList()
		{

			ClearList();


			string sql = "SELECT " +
				"* " +
			"FROM " +
				"Tasks Tsk ";

			List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);


			GameObject prefab;
			foreach (Tasks task in taskList)
			{

				switch (task.Type)
                {
					case (int) TypeOfTasks.Clicker:
						prefab = ConstantSingleton.Instance.ItemClicker;
						break;

					case (int) TypeOfTasks.Task:
						prefab = ConstantSingleton.Instance.ItemTask;
						break;

					case (int) TypeOfTasks.Timer:
						prefab = ConstantSingleton.Instance.ItemTimer;
						break;

					default:
						continue;
                }

				var newItem = Pooler.Instance.Spawn(PoolType.Entities, prefab, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);

				var itemMan = newItem.GetComponent<ItemOfList>();
				itemMan.Init(task);

				_tasks.Add(itemMan);

			}

		}



        private void ClearList()
        {
            
			foreach (ItemOfList task in _tasks)
            {
				Pooler.Instance.Despawn(PoolType.Entities, task.gameObject);
            }

			_tasks.Clear();

        }



        ~ListOfTasks()
		{
			Signals.Remove(this);
		}

	}

}
