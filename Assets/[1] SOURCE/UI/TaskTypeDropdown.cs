using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTypeDropdown : Dropdown<TaskTypeDDItem>
{
    [SerializeField] private TaskForm _taskForm;

    public override void SelectDDItem(TaskTypeDDItem item)
    {
        _selectedItem = item;
        _taskForm.CreateTask(_selectedItem.TaskType);
    }

    protected override void AddItemToList(TaskTypeDDItem item)
    {
        var newDDItem = Pooler.Instance.Spawn(PoolType.Entities, itemPrefab, default, default, listFolder);

        newDDItem.GetComponent<DropDownItem>().Init(item.name, item.color, item.sprite, item.TaskType);
    }
}

[System.Serializable]
public class TaskTypeDDItem : BaseDDItem
{
    public TypeOfTasks TaskType;
}
