using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecoveryTaskItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _dateOfDeletion;

    public void Recovery()
    {

    }

    public void SetInfo(string taskName, DateTime dateOfDeletion)
    {
        _taskName.text = taskName;
        _dateOfDeletion.text = dateOfDeletion.Date.ToShortDateString(); 
    }
}
