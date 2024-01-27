using Germanenko.Source;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DropdownItemType
{
    Color,
    Task
}

public class DropDownItem : MonoBehaviour
{
    [SerializeField] public string _name { get; private set; }

    [SerializeField] public Color _color { get; private set; }

    [SerializeField] public Sprite _sprite { get; private set; }

    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _nameText;

    public DropdownItemType DropdownItemType;

    public TypeOfTasks TaskType;

    [SerializeField] private TaskTypeDropdown _taskTypeDropDown;

    public void Init(string name, Color color, Sprite sprite)
    {
        _name = name;
        _color = color;
        _sprite = sprite;

        _image.sprite = _sprite;
        _image.color = _color;

        if (_nameText != null)
            _nameText.text = _name;
    }

    public void Init(string name, Color color, Sprite sprite, TypeOfTasks taskType)
    {
        _name = name;
        _color = color;
        _sprite = sprite;
        TaskType = taskType;

        _image.sprite = _sprite;
        _image.color = _color;

        if (_nameText != null)
            _nameText.text = _name;
    }
}
