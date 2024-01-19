using DTT.KeyboardRaiser;
using Germanenko.Source;
using HutongGames.PlayMaker.Actions;
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

    [SerializeField] private DropShadow _dropShadow;

    [SerializeField] private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _inputField.onDeselect.AddListener(Deselected);
        _inputField.onSelect.AddListener(Selected);

        _inputField.text = transform.GetSiblingIndex().ToString();
    }



    public void FocusOnInputField()
    {
        StartCoroutine(ConstantSingleton.Instance.TaskFormScroll.FocusOnItemCoroutine(_rectTransform, 2f, new Vector2(0, -500)));
    }



    public void OpenNextInputField()
    {
        var nextInputField = transform.parent.GetChild(transform.GetSiblingIndex() + 1);

        if (nextInputField.TryGetComponent(out InputFieldWithHits inputFieldWithHits))
        {
            inputFieldWithHits.FocusOnInputField();
            inputFieldWithHits.InputField.ActivateInputField();
            inputFieldWithHits.InputField.caretPosition = inputFieldWithHits.InputField.text.Length;
        }
    }



    //private void OnDisable()
    //{
    //    _inputField.onDeselect.RemoveListener(Deselected);
    //}



    public void Selected(string s)
    {
        _dropShadow.Fade(.7f, .2f);
        FocusOnInputField();

#if UNITY_EDITOR
        EditorKeyboard.Open();
#endif
    }



    public void Deselected(string s)
    {
        if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(null);
        
        _dropShadow.Fade(0f, .2f);

#if UNITY_EDITOR
        EditorKeyboard.Close();
#endif
    }
}
