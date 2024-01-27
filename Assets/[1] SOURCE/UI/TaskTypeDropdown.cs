using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTypeDropdown : Dropdown<TaskTypeDDItem>
{
    [SerializeField] private TaskForm _taskForm;

    public override void SelectDDItem(string name)
    {
        _selectedItem.name = name;
        foreach (var item in listOfItems)
        {
            if (item.name == name)
            {
                _selectedItem.color = item.color;
                _selectedItem.sprite = item.sprite;
                _selectedItem.TaskType = item.TaskType;
            }
        }
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
