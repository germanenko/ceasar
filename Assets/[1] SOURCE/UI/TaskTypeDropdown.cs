using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTypeDropdown : Dropdown<TaskTypeDDItem>
{
    [SerializeField] private TaskForm _taskForm;

    [SerializeField] private Image _buttonImage;

    public override void SelectDDItem(string name)
    {
        SelectedItem.name = name;
        foreach (var item in listOfItems)
        {
            if (item.name == name)
            {
                SelectedItem.color = item.color;
                SelectedItem.sprite = item.sprite;
                SelectedItem.TaskType = item.TaskType;
            }
        }

        _taskForm.SetOpenPositionAndColor(transform.localPosition, _buttonImage.color);

        _taskForm.CreateTask(SelectedItem.TaskType);
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
