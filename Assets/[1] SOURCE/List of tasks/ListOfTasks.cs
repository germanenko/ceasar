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
        public List<ItemOfList> Tasks => _tasks;



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


			string sql = "SELECT * FROM Tasks Tsk";

			List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            string sqlPriority = $"SELECT * FROM Priority";

            List<Priority> taskPriorities = ConstantSingleton.Instance.DbManager.Query<Priority>(sqlPriority);

            GameObject prefab;
			//foreach (Tasks task in taskList)
			//{

			//	switch (task.Type)
   //             {
			//		case (int) TypeOfTasks.Clicker:
			//			prefab = ConstantSingleton.Instance.ItemClicker;
			//			break;

			//		case (int) TypeOfTasks.Task:
			//			prefab = ConstantSingleton.Instance.ItemTask;
			//			break;

			//		case (int) TypeOfTasks.Timer:
			//			prefab = ConstantSingleton.Instance.ItemTimer;
			//			break;

   //                 default:
			//			continue;
   //             }

			//	var newItem = Pooler.Instance.Spawn(PoolType.Entities, prefab, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
   //             Debug.Log("добавлен таск");

   //             var itemMan = newItem.GetComponent<ItemOfList>();

			//	if(task.Draft == true)
			//		itemMan.SetDraft(true);

			//	int priority = taskPriority[taskList]

			//	itemMan.Init(task);

			//	_tasks.Add(itemMan);
			//}



			for (int i = 0; i < taskList.Count; i++)
			{
                switch (taskList[i].Type)
                {
                    case (int)TypeOfTasks.Clicker:
                        prefab = ConstantSingleton.Instance.ItemClicker;
                        break;

                    case (int)TypeOfTasks.Task:
                        prefab = ConstantSingleton.Instance.ItemTask;
                        break;

                    case (int)TypeOfTasks.Timer:
                        prefab = ConstantSingleton.Instance.ItemTimer;
                        break;

                    default:
                        continue;
                }

                var newItem = Pooler.Instance.Spawn(PoolType.Entities, prefab, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);

                var itemMan = newItem.GetComponent<ItemOfList>();

                if (taskList[i].Draft == true)
                    itemMan.SetDraft(true);

                int priority = 0;
                foreach (var taskPriority in taskPriorities)
                {
                    if(taskPriority.TaskID == taskList[i].ID)
                    {
                        priority = taskPriority.PriorityValue;
                    }
                }

                itemMan.transform.SetSiblingIndex(priority-1);

                itemMan.Init(taskList[i], priority);

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



        public int CountOfDrafts()
        {
            int draftCount = 0;

            foreach (var task in _tasks)
            {
                if(task.IsDraft)
                    draftCount++;
            }

            return draftCount;
        }



        ~ListOfTasks()
		{
			Signals.Remove(this);
		}

	}

}
