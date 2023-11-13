#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Germanenko.Framework;
using Germanenko.Source;




public class Starter : MonoBehaviour
{

    [SerializeField] private List<ScriptableObject> Controllers = new List<ScriptableObject>();
    [SerializeField] private Constants constManager;

    public Canvas[] Canvases;

    public void Start()
    {
        Application.targetFrameRate = 60;

        foreach (var canvas in Canvases)
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }

        if (Updater.Default == null) Updater.Create();


        // add controllers from interface
        foreach(var controller in Controllers) 
            Toolbox.Add(controller);


        constManager.Init();


        Toolbox.Add<ListOfTasks>();
        Toolbox.Add<Tables>();
        Toolbox.Add<CameraShake>();


        Toolbox.Get<Tables>().Init();

        Toolbox.Get<ListOfTasks>().CreatePoolTasks();

        Toolbox.Get<ListOfTasks>().ReloadList();

    }
}


