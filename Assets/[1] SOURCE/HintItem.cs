using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;


namespace Germanenko.Source
{

    public class HintItem : MonoBehaviour
    {

        public HintItem previousItem;
        public Image img;

        [SerializeField] private TextMeshProUGUI _text;
        
        private Dictionary<Image, Color> _OldColors = new();
        private Hints parentScript;



        public void Init()
        {

            _OldColors.Add(img, img.color);

            var prev = previousItem;
            while (prev != null){
                _OldColors.Add(prev.img, prev.img.color);
                prev = prev.previousItem;            
            } 

        }



        public void SetText(string newText, Hints caller)
        {

            _text.text = newText;
            parentScript = caller;

            Init();

        }



        public string Text() => _text.text;



        //
        // for playmaker
        //

        public void AddHints()
        {
            parentScript.AddHints(this);
        }



        public void SelectHints(Color newColor)
        {

            foreach (var item in _OldColors)
                item.Key.color = newColor;

        }



        public void UnselectHints()
        {

            foreach (var item in _OldColors)
                item.Key.color = item.Value;

        }

    }

}
