using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Framework;
using TMPro;
using DTT.KeyboardRaiser;
using UnityEngine.UI;
using AdvancedInputFieldPlugin;

namespace Germanenko.Source
{

    public class Hints : MonoBehaviour
    {

        [SerializeField] private GameObject _hint;
        [SerializeField] private TMP_InputField _taskName;
        [SerializeField] private AdvancedInputField _inputField;
        [SerializeField] private string _hintTitle;

        private List<HintItem> listOfHints = new();

        public void Start()
        {
            var parsedHint = _hintTitle.Split(' ');
            HintItem hintItem = null;


            foreach (var item in parsedHint)
            {

                GameObject newHint = Pooler.Instance.Spawn(PoolType.Entities, _hint, default, default, transform);
                var newHintItem = newHint.GetComponentInChildren<HintItem>();

                if (hintItem != null) newHintItem.previousItem = hintItem;

                hintItem = newHintItem;
                hintItem.SetText(item, this);

                listOfHints.Add(hintItem);
            }

            //KeyboardStateManager.openingField.Add(GetComponent<UIKeyboardRaiser>());

            //foreach (var hint in listOfHints)
            //{
            //    hint.gameObject.SetActive(false);
            //}
        }

        private void OnEnable()
        {
            KeyboardStateManager.Current.Raised += OnRaised;
            KeyboardStateManager.Current.Lowered += OnLowered;
        }

        public void OnRaised()
        {
            //SetActiveHints(true);
            _taskName = KeyboardStateManager.openingField[0].GetComponent<InputFieldWithHits>().InputField;

        }
        public void OnLowered()
        {
            //SetActiveHints(false);
            _taskName = null;
        }

        //private void SetActiveHints(bool active)
        //{
        //    foreach (var hint in listOfHints)
        //    {
        //        hint.gameObject.SetActive(active);
        //    }
        //}

        public void AddHints(HintItem selectedItem)
        {

            string str = "";


            foreach (var item in listOfHints)
            {

                str += " " + item.Text();
                if (item == selectedItem) break;

            }


            _inputField.Text += str;

            MoveToEndOfLine();

        }



        public void MoveToEndOfLine()
        {

            //var inputField = transform.parent.GetComponentInChildren<TMP_InputField>();

            //_taskName.Select();
            //_taskName.MoveToEndOfLine(false, false);

            _inputField.SetCaretToTextEnd();

            //inputField?.MoveToEndOfLine(false, false);
            //inputField?.ActivateInputField();

        }



        //
        // for playmaker
        //

        public void AddHints()
        {
            _inputField.Text += _hintTitle;
        }



        public void DeleteLastHint()
        {

            string str = _taskName.text;
            int i;

            for (i = str.Length - 1; (str[i] != ' ' || str[i - 1] == ' ') && i > 0; i--) ;

            _inputField.Text = str.Substring(0, i);

            MoveToEndOfLine();

        }

    }

}

