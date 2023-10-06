using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTaskList : MonoBehaviour
{
    public TaskView TaskView;

    public void FillTaskList()
    {
        foreach (var child in transform.GetComponentsInChildren<TaskView>())
        {
            Destroy(child.gameObject);
        }

        string sql = "SELECT * FROM Tasks";

        List<Tasks> taskList = ConstantSingleton.Instance.DbManager.Query<Tasks>(sql);

        foreach (var task in taskList) 
        {
            var t = Instantiate(TaskView, transform);
            t.TaskText.text = $"{task.ID} - Name: {task.Name} - Color: {task.Color} - Draft: {task.Draft} - Type: {task.Type}";
        }
    }
}
