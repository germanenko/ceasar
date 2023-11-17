using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Germanenko.Source
{

    public class Constants : MonoBehaviour
    {

        [Header("Folders")]
        [SerializeField] private Transform FolderDinamicElements;
        [SerializeField] private Transform FolderListOfItems;


        [Header("FormManagers")]
        [SerializeField] private TaskForm TaskFormManager;


        [Header("Items")]
        [SerializeField] private GameObject ItemClicker;
        [SerializeField] private GameObject ItemTask;
        [SerializeField] private GameObject ItemTimer;


        [Header("Others")]
        [SerializeField] private Transform MainCamera;
        [SerializeField] private SimpleSQL.SimpleSQLManager DbManager;
        [SerializeField] private GameObject MultiChoiceScreen;
        [SerializeField] private ScrollRect TaskFormScroll;

        public void Init()
        {

            InitFolders();
            InitFormManager();
            InitItems();
            InitOthers();
        }



        private void InitFormManager()
        {
            ConstantSingleton.Instance.TaskFormManager = TaskFormManager;
        }



    private void InitFolders()
        {
            ConstantSingleton.Instance.FolderDinamicElements = FolderDinamicElements;
            ConstantSingleton.Instance.FolderListOfItems = FolderListOfItems;
        }



        private void InitItems()
        {
            ConstantSingleton.Instance.ItemClicker = ItemClicker;
            ConstantSingleton.Instance.ItemTimer = ItemTimer;
            ConstantSingleton.Instance.ItemTask = ItemTask;
        }



        private void InitOthers()
        {
            ConstantSingleton.Instance.MainCamera = MainCamera;
            ConstantSingleton.Instance.DbManager = DbManager;
            ConstantSingleton.Instance.MultiChoiceScreen = MultiChoiceScreen;
            ConstantSingleton.Instance.TaskFormScroll = TaskFormScroll;
        }
    }

}