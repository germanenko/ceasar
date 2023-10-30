using Doozy.Runtime.Common.Extensions;
using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryDeletedTasks : MonoBehaviour
{
    [SerializeField] private RecoveryTaskItem _deletedTaskPrefab;
    [SerializeField] private Transform _deletedTasksList;

    public void LoadDeletedTasks()
    {
        var children = _deletedTasksList.GetChildren();

        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        string deletedTasksSql = $"SELECT * FROM DeletedTasks";

        var deletedTasks = ConstantSingleton.Instance.DbManager.Query<DeletedTasks>(deletedTasksSql);

        foreach (var deletedTask in deletedTasks)
        {
            var item = Pooler.Instance.Spawn(PoolType.Entities, _deletedTaskPrefab.gameObject, default, default, _deletedTasksList);

            var task = ConstantSingleton.Instance.DbManager.Query<Tasks>($"SELECT * FROM Tasks WHERE ID = {deletedTask.TaskID}");

            item.GetComponent<RecoveryTaskItem>().SetInfo(task[0].ID, task[0].Name, deletedTask.Date);
        }
    }
}
