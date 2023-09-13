using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownItem : MonoBehaviour
{
    [SerializeField] public string _name { get; private set; }

    [SerializeField] public Color _color { get; private set; }

    [SerializeField] public Sprite _sprite { get; private set; }

    [SerializeField] private Image _image;

    public void Init(string name, Color color, Sprite sprite)
    {
        _name = name;
        _color = color;
        _sprite = sprite;

        _image.sprite = _sprite;
        _image.color = _color;
    }
}
