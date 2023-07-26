using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



namespace Germanenko.Source
{

    public class ItemOfList : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI ID;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Times;

        [SerializeField] private Image TaskColor;



        public void Init(Tasks _data)
        {
            Color currentColor;
            ColorUtility.TryParseHtmlString("#" + _data.Color, out currentColor);
            //TaskColor.color =  currentColor;

            ID.text = _data.ID.ToString();
            Title.text = _data.Name; 
        }

    }

}