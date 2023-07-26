using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Germanenko.Source
{

    public class Question : MonoBehaviour
    {

        public string questionName;

        [Header("Labels")]
        public string message = "Question message";

        [Header("Buttons")]
        public string buttonOkName = "OK";
        public Sprite buttonOkIcon;
        public UnityEvent buttonOkEvent = new UnityEvent();

        [Space(2)]
        public string buttonCancelName = "Cancel";
        public Sprite buttonCancelIcon;
        public UnityEvent buttonCancelEvent = new UnityEvent();



        public void Show()
        {

            UIPopup
                .Get(questionName)
                .SetTexts(message, buttonOkName, buttonCancelName)
                .SetSprites(buttonOkIcon, buttonCancelIcon)
                .SetEvents(buttonOkEvent, buttonCancelEvent)
                .Show();

        }

    }

}
