using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Framework;

namespace Germanenko.Source
{

    public class TopMenu : MonoBehaviour
    {

        public void ClearTasks()
        {
            Toolbox.Get<Tables>().DropTable();
            //ConstantSingleton.Instance.NotificationManager.InitWindow(iconMenuSystem, "All OK", "Task list is empty");***
            Toolbox.Get<Tables>().Init();
            Toolbox.Get<ListOfTasks>().ReloadList();
        }

        public void SetLocalize(bool _russia)
        {
            PlayerPrefs.SetInt("LocalizeRussia", _russia ? 1 : 0);
        }

    }

}