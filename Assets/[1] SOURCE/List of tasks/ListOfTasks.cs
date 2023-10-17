using Germanenko.Framework;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleSQL;
using System.Threading.Tasks;

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


            string sql = "SELECT * FROM Tasks WHERE Reference = 0";

            List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

            string sqlPriority = $"SELECT * FROM Priority";

            List<Priority> taskPriorities = ConstantSingleton.Instance.DbManager.Query<Priority>(sqlPriority);

            GameObject prefab = ConstantSingleton.Instance.ItemClicker;
            //foreach (Tasks task in taskList)
            //{

            //  switch (task.Type)
            //             {
            //    case (int) TypeOfTasks.Clicker:
            //      prefab = ConstantSingleton.Instance.ItemClicker;
            //      break;

            //    case (int) TypeOfTasks.Task:
            //      prefab = ConstantSingleton.Instance.ItemTask;
            //      break;

            //    case (int) TypeOfTasks.Timer:
            //      prefab = ConstantSingleton.Instance.ItemTimer;
            //      break;

            //                 default:
            //      continue;
            //             }

            //  var newItem = Pooler.Instance.Spawn(PoolType.Entities, prefab, default(Vector3), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);
            //             Debug.Log("добавлен таск");

            //             var itemMan = newItem.GetComponent<ItemOfList>();

            //  if(task.Draft == true)
            //    itemMan.SetDraft(true);

            //  int priority = taskPriority[taskList]

            //  itemMan.Init(task);

            //  _tasks.Add(itemMan);
            //}


            for (int i = 0; i < taskPriorities.Count; i++)
            {
                string sqlP = $"SELECT * FROM Tasks WHERE ID = {taskPriorities[i].TaskID}";

                Tasks t;
                try
                {
                    t = ConstantSingleton.Instance.DbManager.Query<Tasks>(sqlP)[0];
                }
                catch (Exception e)
                {
                    Debug.LogError("Для продолжения требуется сбросить базу данных");
                    break;
                    throw;
                }


                switch (t.Type)
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

                var newItem = Pooler.Instance.Spawn(PoolType.Entities, prefab, new Vector3(Screen.width/2, -Screen.height/2, 0), default(Quaternion), ConstantSingleton.Instance.FolderListOfItems);

                var itemMan = newItem.GetComponent<ItemOfList>();

                if (taskList[i].Draft == true)
                    itemMan.SetDraft(true);

                var priority = taskPriorities[i].PriorityValue;

                itemMan.Init(t, priority);

                if (i < taskPriorities.Count - 1)
                    newItem.GetComponent<LerpToPlaceholder>().InstantlyMove = true;

                _tasks.Add(itemMan);
            }
        }



        public void SetMultiChoice(bool multiChoice)
        {
            ConstantSingleton.Instance.MultiChoiceScreen.SetActive(multiChoice);
            foreach (var task in _tasks)
            {
                task.SetCanSelect(multiChoice);
                task.CheckBox.OnToggleOnCallback.Event.AddListener(CountSelectedTasks);
                task.CheckBox.OnToggleOffCallback.Event.AddListener(CountSelectedTasks);
            }
        }



        private void CountSelectedTasks()
        {
            int count = 0;
            foreach (var task in _tasks)
            {
                if(task.Selected) count++;
            }
            Debug.Log(count);
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
                if (task.IsDraft)
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