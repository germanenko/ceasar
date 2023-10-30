using Germanenko.Framework;
using Germanenko.Source;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecoveryTaskItem : MonoBehaviour
{
    private int _taskID;
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _dateOfDeletion;

    public void Recovery()
    {
        print(_taskID);
        Toolbox.Get<Tables>().RecoveryTask(_taskID);
    }

    public void SetInfo(int taskID, string taskName, DateTime dateOfDeletion)
    {
        _taskID = taskID;
        _taskName.text = taskName;
        _dateOfDeletion.text = dateOfDeletion.Date.ToShortDateString(); 
    }
}
