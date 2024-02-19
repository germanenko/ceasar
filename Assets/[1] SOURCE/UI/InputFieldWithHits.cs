using AdvancedInputFieldPlugin;
using DTT.KeyboardRaiser;
using Germanenko.Source;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldWithHits : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    public TMP_InputField InputField => _inputField;

    [SerializeField] private AdvancedInputField _advancedInputField;
    public AdvancedInputField AdvancedInputField => _advancedInputField;

    [SerializeField] private DropShadow _dropShadow;

    [SerializeField] private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if(_advancedInputField != null)
        {
            _advancedInputField.OnSelectionChanged.AddListener(SelectChange);
            return;
        }

        _inputField.onDeselect.AddListener(Deselected);
        _inputField.onSelect.AddListener(Selected);

        //_inputField.text = transform.GetSiblingIndex().ToString();
    }



    public void FocusOnInputField()
    {
        StartCoroutine(ConstantSingleton.Instance.TaskFormScroll.FocusOnItemCoroutine(_rectTransform, 2f, new Vector2(0, -500)));
    }



    public void OpenNextInputField(string empty)
    {
        _inputField.onEndEdit.RemoveListener(OpenNextInputField);

        Transform nextInputField = null;

        try
        {
            nextInputField = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
        }
        catch(Exception e)
        {
            _inputField.DeactivateInputField();
            return;
        }

        if (nextInputField.TryGetComponent(out InputFieldWithHits inputFieldWithHits))
        {
            inputFieldWithHits.FocusOnInputField();
            inputFieldWithHits.InputField.ActivateInputField();
            inputFieldWithHits.InputField.caretPosition = inputFieldWithHits.InputField.text.Length;
        }
    }



    public void OpenNextAdvancedInputField(string result, EndEditReason reason)
    {
        _advancedInputField.OnEndEdit.RemoveListener(OpenNextAdvancedInputField);

        try
        {
            if (transform.parent.GetChild(transform.GetSiblingIndex() + 1) == null) return;
        }
        catch
        {
            return;
        }

        var nextInputField = transform.parent.GetChild(transform.GetSiblingIndex() + 1);

        if (nextInputField.TryGetComponent(out InputFieldWithHits inputFieldWithHits))
        {
            print("next");
            inputFieldWithHits.FocusOnInputField();
            inputFieldWithHits.AdvancedInputField.Select();
            inputFieldWithHits.AdvancedInputField.GetEngine.LoadKeyboard();
            inputFieldWithHits.AdvancedInputField.GetEngine.KeyboardClient.Activate();
            inputFieldWithHits.AdvancedInputField.SetCaretToTextEnd();
        }
    }



    //private void OnDisable()
    //{
    //    _inputField.onDeselect.RemoveListener(Deselected);
    //}



    public void Selected(string s)
    {
        if(_dropShadow)
            _dropShadow.Fade(.7f, .2f);

        FocusOnInputField();

        _inputField.onEndEdit.AddListener(OpenNextInputField);

#if UNITY_EDITOR
        EditorKeyboard.Open();
#endif
    }



    public void Deselected(string s)
    {
        if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(null);

        if (_dropShadow)
            _dropShadow.Fade(0f, .2f);

        _inputField.onEndEdit.RemoveListener(OpenNextInputField);

#if UNITY_EDITOR
        EditorKeyboard.Close();
#endif
    }



    public void SelectChange(bool change)
    {
        if(change)
        {
            if (_dropShadow)
                _dropShadow.Fade(.7f, .2f);

            FocusOnInputField();

            //_advancedInputField.OnEndEdit.AddListener(OpenNextAdvancedInputField);

#if UNITY_EDITOR
            EditorKeyboard.InvokeOpen();
#endif
        }
        else
        {
            if (_dropShadow)
                _dropShadow.Fade(0f, .2f);

            //_advancedInputField.OnEndEdit.RemoveListener(OpenNextAdvancedInputField);

#if UNITY_EDITOR
            EditorKeyboard.InvokeClose();
#endif
        }
    }

}
