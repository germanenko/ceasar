using Doozy.Runtime.UIManager.Components;
using Germanenko.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Calendar : MonoBehaviour
{
    [SerializeField] private Dictionary<int, string> _months = new Dictionary<int, string> { 
    { 1, "January" },
    { 2, "February" },
    { 3, "March" },
    { 4, "April" },
    { 5, "May" },
    { 6, "June" },
    { 7, "Jule" },
    { 8, "August" },
    { 9, "September" },
    { 10, "October" },
    { 11, "November" },
    { 12, "December" }};

    [SerializeField] private int _year;

    [SerializeField] private int _month;

    [SerializeField] private int _day;

    [SerializeField] private DayButton _dayButtonPrefab;

    [SerializeField] private Transform _dayButtonsParent;

    [SerializeField] private List<DayButton> _dayButtons;
    [SerializeField] private List<UIToggle> _monthToggles;

    [SerializeField] private TextMeshProUGUI _dateText;
    [SerializeField] private TextMeshProUGUI _dateTextInButton;

    [SerializeField] private DateTime _date;

    [SerializeField] private UIStepper _yearStepper;

    [SerializeField] private GameObject _calendarWindow;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _weekendColor;
    [SerializeField] private Color _otherMonthColor;


    private void Start()
    {
        _day = DateTime.Now.Day;
        _month = DateTime.Now.Month;
        _year = DateTime.Now.Year;

        _yearStepper.SetValue(_year);

        _monthToggles[_month - 1].isOn = true;

        _date = DateTime.Now;

        _dateTextInButton.text = _date.Date.ToShortDateString();

        _yearStepper.OnValueChanged.AddListener(SetYear);
    }

    public void OpenCalendarWindow()
    {
        _calendarWindow.SetActive(true);

        GenerateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }

    public void CloseCalendarWindow()
    {
        _calendarWindow.SetActive(false);
    }

    public void SetYear(float year)
    {
        _year = (int)year;
        UpdateDate();
    }



    public void SetMonth(int month)
    {
        _month = month;
        UpdateDate();
    }



    public void SetDay(int day)
    {
        _day = day;
        UpdateDate();
    }



    private void UpdateDate()
    {
        _date = new DateTime(_year, _month, _day);
        _dateText.text = _date.Date.ToShortDateString();
    }



    private void GenerateCalendar(int year, int month)
    {
        if (_dayButtons.Count > 0) return;

        foreach (var item in _dayButtons)
        {
            Pooler.Instance.Despawn(PoolType.Entities, item.gameObject);
        }

        _dayButtons.Clear();



        for (int i = 0; i < GetMonthStartDay(year, month); i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1);
            dayButton.SetInteractable(false);
            dayButton.SetCalendar(this);
            dayButton.SetColor(_otherMonthColor);

            if (new DateTime(year, month, i).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetColor(_weekendColor);
            }

            _dayButtons.Add(dayButton);
        }



        for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(i + 1);
            dayButton.SetInteractable(true);
            dayButton.SetCalendar(this);
            dayButton.SetColor(_defaultColor);

            if(new DateTime(year, month, i).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetColor(_weekendColor);
            }

            _dayButtons.Add(dayButton);
        }

        int lastDays = 42 - _dayButtons.Count;

        for (int i = 0; i < lastDays; i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(i + 1);
            dayButton.SetInteractable(false);
            dayButton.SetCalendar(this);
            dayButton.SetColor(_otherMonthColor);

            if (new DateTime(year, month, i).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetColor(_weekendColor);
            }

            _dayButtons.Add(dayButton);
        }
    }



    public void ConfirmDate()
    {
        _dateTextInButton.text = _date.ToShortDateString();
        CloseCalendarWindow();
    }



    private DayButton SpawnDayButton()
    {
        var d = Pooler.Instance.Spawn(PoolType.Entities, _dayButtonPrefab.gameObject, default, default, _dayButtonsParent);
        DayButton dayButton = d.GetComponent<DayButton>();
        return dayButton;
    }



    private int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        return (int)temp.DayOfWeek - 1;
    }
}
