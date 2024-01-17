using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    [SerializeField] private Calendar _calendar;

    [SerializeField] private int _day;
    [SerializeField] private DateTime _dayInDateTime;

    [SerializeField] private TextMeshProUGUI _dayText;
    public TextMeshProUGUI DayText => _dayText;

    [SerializeField] private Image _image;

    public UIToggle UIToggle;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _weekendColor;
    [SerializeField] private Color _otherMonthColor;
    [SerializeField] private Color _currentColor;

    [SerializeField] private UIToggleColorAnimator _animator;

    [SerializeField] private bool _nextMonth;
    [SerializeField] private bool _previousMonth;
    [SerializeField] private bool _isWeekend;

    public void SetDay(DateTime date)
    {
        _day = date.Day;
        _dayText.text = _day.ToString();
        _dayInDateTime = date;
    }

    

    public void SetNextAndPreviousMonth(bool next, bool previous)
    {
        _nextMonth = next;
        _previousMonth = previous;

        if(next || previous)
        {
            SetColor(_otherMonthColor);
        }
    }



    public void SetWeekend(bool weekend)
    {
        _isWeekend = weekend;
        
        if(weekend)
            SetColor(_weekendColor);
    }



    public void SetCalendar(Calendar calendar)
    {
        _calendar = calendar;
    }



    public void SelectDay()
    {
        _calendar.SetDay(_dayInDateTime);
    }



    public void SetColor(Color color)
    {
        _image.color = color;
        _currentColor = color;
    }



    public void SetStartColor()
    {
        _animator.SetStartColorForOff(_currentColor);
    }



    public void DeselectDay()
    {
        SetColor(_currentColor);
    }



    public void Drop()
    {
        SetNextAndPreviousMonth(false, false);
        SetWeekend(false);
        DayText.fontStyle = FontStyles.Normal;
        SetColor(_defaultColor);

        if (UIToggle.isOn)
            UIToggle.isOn = false;
    }
}
