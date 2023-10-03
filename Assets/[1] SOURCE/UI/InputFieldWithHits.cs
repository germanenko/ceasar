using DTT.KeyboardRaiser;
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

    private void OnEnable()
    {
        _inputField.onDeselect.AddListener(Deselected);
        _inputField.onSelect.AddListener(Selected);

        _inputField.text = transform.GetSiblingIndex().ToString();
    }



    private void OnDisable()
    {
        _inputField.onDeselect.RemoveListener(Deselected);
    }



    public void Selected(string s)
    {
#if UNITY_EDITOR
        EditorKeyboard.Open();
#endif
    }



    public void Deselected(string s)
    {
        if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(null);
#if UNITY_EDITOR
        EditorKeyboard.Close();
#endif
    }
}
