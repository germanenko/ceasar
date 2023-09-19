using DTT.KeyboardRaiser;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputFieldWithHits : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    public TMP_InputField InputField => _inputField;
}
