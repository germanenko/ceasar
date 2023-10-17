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
            t.N1.text = task.ID.ToString();
            t.N2.text = task.ID.ToString();
            t.Name.text = task.Name.ToString();
            t.Draft.text = task.Draft.ToString();
            t.Reference.text = task.Reference.ToString();
        }
    }
}
