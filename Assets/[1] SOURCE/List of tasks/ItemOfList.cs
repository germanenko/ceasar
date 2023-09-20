using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;

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
    }

}