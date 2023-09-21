using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;

namespace Germanenko.Source
{

    public class ItemOfList : MonoBehaviour
    {
        [SerializeField] private int _id;

        [SerializeField] private TextMeshProUGUI ID;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Times;

        [SerializeField] private Image TaskColor;

        [SerializeField] private Image _icon;
        public bool IsDraft;

        [SerializeField] private TaskForm _taskForm;

        public bool isDragging;
        [SerializeField] private Transform _taskToReplace;

        [SerializeField] private LayoutElement _layoutElement;

        public void Init(Tasks _data)
        {
            _taskForm = FindObjectOfType<TaskForm>();

            Color currentColor;
            ColorUtility.TryParseHtmlString("#" + _data.Color, out currentColor);
            //TaskColor.color =  currentColor;

            _id = _data.ID;
            ID.text = _data.ID.ToString();
            Title.text = _data.Name;

            if (IsDraft)
                _icon.gameObject.SetActive(true);
        }



        public void SendID()
        {
            _taskForm.SetTaskID(_id, IsDraft);
            
            if(!IsDraft)
                _taskForm.SetEditTask(true);
        }



        public void SetDraft(bool draft)
        {
            IsDraft = draft;
            _icon.gameObject.SetActive(draft);
        }


        private void OnDisable()
        {
            print("отключен");
            SetDraft(false);
        }



        public void OnTriggerEnter2D(Collider2D col)
        {
            print("enter");
            if(TryGetComponent(out ItemOfList iol))
            {
                if (!col.GetComponent<ItemOfList>().isDragging)
                {
                    _taskToReplace = col.transform;
                }
            }
        }



        public void OnTriggerExit2D(Collider2D col)
        {
            print("exit");
            _taskToReplace = null;
        }



        public void ReplaceTask()
        {
            if (_taskToReplace)
                transform.SetSiblingIndex(_taskToReplace.GetSiblingIndex());
        }

    }

}