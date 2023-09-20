using Germanenko.Framework;
using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseActions : MonoBehaviour
{
    public void ClearDraft()
    {
        Toolbox.Get<Tables>().DropDraft();
    }



    public void ReloadTaskList()
    {
        Toolbox.Get<ListOfTasks>().ReloadList();
    }
}
