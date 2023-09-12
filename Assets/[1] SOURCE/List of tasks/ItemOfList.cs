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



        public void Init(Tasks _data)
        {
            Color currentColor;
            ColorUtility.TryParseHtmlString("#" + _data.Color, out currentColor);
            //TaskColor.color =  currentColor;

            _id = _data.ID;
            ID.text = _data.ID.ToString();
            Title.text = _data.Name;
        }



        public void SendID()
        {
            var receiver = FindObjectOfType<TaskForm>();
            receiver.SetTaskID(_id);
        }

    }

}