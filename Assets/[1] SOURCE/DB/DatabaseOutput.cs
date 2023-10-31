using Doozy.Runtime.Common.Extensions;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseOutput : MonoBehaviour
{
    [SerializeField] private Transform _taskList;
    [SerializeField] private Transform _savesAndDraftsList;
    [SerializeField] private Transform _deletedTasks;

    [SerializeField] private TaskView _taskView;
    [SerializeField] private SavesAndDraftsView _savesAndDraftsView;
    [SerializeField] private DeletedTaskView _deletedTaskView;

    public void LoadTasks()
    {
        var children = _taskList.GetChildren();

        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        string tasksSql = $"SELECT * FROM Tasks";
        var tasks = ConstantSingleton.Instance.DbManager.Query<Tasks>(tasksSql);

        for (int i = 0; i < tasks.Count; i++)
        {
            var t = Instantiate(_taskView, _taskList);
            t.ID.text = tasks[i].ID.ToString();
            t.Name.text = tasks[i].Name;
            t.Color.text = tasks[i].Color.ToString();
        }
    }

    public void LoadSavesAndDrafts()
    {
        var children = _savesAndDraftsList.GetChildren();

        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        string savesSql = $"SELECT * FROM SavesAndDrafts";
        var saves = ConstantSingleton.Instance.DbManager.Query<SavesAndDrafts>(savesSql);

        for (int i = 0; i < saves.Count; i++)
        {
            var t = Instantiate(_savesAndDraftsView, _savesAndDraftsList);
            t.ID.text = saves[i].ID.ToString();
            t.TaskID.text = saves[i].TaskID.ToString();
            t.Draft.text = saves[i].Draft.ToString();
            t.Reference.text = saves[i].Reference.ToString();
        }
    }

    public void LoadDeletedTasks()
    {
        var children = _deletedTasks.GetChildren();

        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        string deletedTasksSql = $"SELECT * FROM DeletedTasks";
        var deletedTasks = ConstantSingleton.Instance.DbManager.Query<DeletedTasks>(deletedTasksSql);

        for (int i = 0; i < deletedTasks.Count; i++)
        {
            var t = Instantiate(_deletedTaskView, _deletedTasks);
            t.ID.text = deletedTasks[i].ID.ToString();
            t.TaskID.text = deletedTasks[i].TaskID.ToString();
            t.Date.text = deletedTasks[i].Date.ToShortDateString();
        }
    }
}
