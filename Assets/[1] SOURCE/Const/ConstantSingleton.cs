using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Framework;
using System;
using UnityEngine.UI;

namespace Germanenko.Source
{

    public class ConstantSingleton : Singleton<ConstantSingleton>
    {

        #region FOLDERS

        private Transform folderDinamicElements;
        private Transform folderListOfItems;


        public Transform FolderDinamicElements { get => folderDinamicElements; set => folderDinamicElements = value; }
        public Transform FolderListOfItems { get => folderListOfItems; set => folderListOfItems = value; }

        #endregion



        #region ITEMS

        private GameObject itemClicker;
        private GameObject itemTask;
        private GameObject itemTimer;


        public GameObject ItemClicker { get => itemClicker; set => itemClicker = value; }
        public GameObject ItemTask { get => itemTask; set => itemTask = value; }
        public GameObject ItemTimer { get => itemTimer; set => itemTimer = value; }

        #endregion



        #region FORM MANAGERS

        private TaskForm taskFormManager;


        public TaskForm TaskFormManager { get => taskFormManager; set => taskFormManager = value; }


        #endregion



        #region CANVAS

        private Canvas canvasTaskForm;


        public Canvas CanvasTaskForm { get => canvasTaskForm; set => canvasTaskForm = value; }

        #endregion



        #region OTHERS

        private Transform mainCamera;
        [SerializeField] private SimpleSQL.SimpleSQLManager dbManager;


        public Transform MainCamera { get => mainCamera; set => mainCamera = value; }
        public SimpleSQL.SimpleSQLManager DbManager { get => dbManager; set => dbManager = value; }

        public GameObject MultiChoiceScreen;

        public ScrollRect TaskFormScroll;

        #endregion



        public void OnEnable()
        {
        }

    }
}