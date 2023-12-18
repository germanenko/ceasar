using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    [SerializeField] private Calendar _calendar;

    [SerializeField] private int _day;

    [SerializeField] private TextMeshProUGUI _dayText;
    public TextMeshProUGUI DayText => _dayText;

    [SerializeField] private Image _image;

    public UIToggle UIToggle;

    [SerializeField] private Color _defaultColor;

    [SerializeField] private UIToggleColorAnimator _animator;

    public void SetDay(int day)
    {
        _day = day;
        _dayText.text = day.ToString();
    }



    public void SetCalendar(Calendar calendar)
    {
        _calendar = calendar;
    }



    public void SelectDay()
    {
        _calendar.SetDay(_day);
    }



    public void SetColor(Color color)
    {
        _image.color = color;
        _defaultColor = color;
    }



    public void SetStartColor()
    {
        _animator.SetStartColorForOff(_defaultColor);
    }



    public void SetInteractable(bool interactable)
    {
        if(interactable)
            _image.color = Color.white;
        else
            _image.color = Color.gray;
    }
}
