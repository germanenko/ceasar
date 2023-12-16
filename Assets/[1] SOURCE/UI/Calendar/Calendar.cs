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
    [SerializeField] private Dictionary<int, List<string>> _months = new Dictionary<int, List<string>> { 
    { 1, new List<string>{"January", "jan" } },
    { 2, new List<string>{"February", "feb" } },
    { 3, new List<string>{"March", "mar" } },
    { 4, new List<string>{"April", "apr" } },
    { 5, new List<string>{"May", "may" } },
    { 6, new List<string>{"June", "jun" } },
    { 7, new List<string>{"Jule", "jul" } },
    { 8, new List<string>{"August", "aug" } },
    { 9, new List<string>{"September", "sep" } },
    { 10, new List<string>{"October", "oct" } },
    { 11, new List<string>{"November", "nov" } },
    { 12, new List<string>{"December", "dec" } } };

    [SerializeField] private int _year;

    [SerializeField] private int _month;

    [SerializeField] private int _day;

    [SerializeField] private DayButton _dayButtonPrefab;
    [SerializeField] private MonthToggle _monthTogglePrefab;

    [SerializeField] private Transform _dayButtonsParent;
    [SerializeField] private Transform _monthTogglesParent;

    [SerializeField] private List<DayButton> _dayButtons;
    [SerializeField] private List<MonthToggle> _monthToggles;

    [SerializeField] private TextMeshProUGUI _dateText;
    [SerializeField] private TextMeshProUGUI _dateTextInButton;

    [SerializeField] private DateTime _date;

    [SerializeField] private UIStepper _yearStepper;

    [SerializeField] private GameObject _calendarWindow;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _weekendColor;
    [SerializeField] private Color _otherMonthColor;

    [SerializeField] private UIToggleGroup _daysToggleGroup;

    private void Start()
    {
        _day = DateTime.Now.Day;
        _month = DateTime.Now.Month;
        _year = DateTime.Now.Year;

        _yearStepper.SetValue(_year);

        _date = DateTime.Now;

        _dateTextInButton.text = _date.Date.ToShortDateString();

        _yearStepper.OnValueChanged.AddListener(SetYear);

        GenerateMonthList();
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

        DateTime previousDate = _date;
        previousDate = previousDate.AddMonths(-1);

        for (int i = 0; i < GetMonthStartDay(year, month); i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1);
            dayButton.SetInteractable(false);
            dayButton.SetCalendar(this);
            dayButton.SetColor(_otherMonthColor);
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);

            if (new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1).DayOfWeek == DayOfWeek.Sunday)
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
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);

            if (new DateTime(year, month, i + 1).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetColor(_weekendColor);
            }

            if(i+1 == _day)
            {
                dayButton.UIToggle.isOn = true;
            }

            _dayButtons.Add(dayButton);
        }

        int lastDays = 42 - _dayButtons.Count;

        DateTime nextDate = _date;
        nextDate = nextDate.AddMonths(1);

        for (int i = 0; i < lastDays; i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(i + 1);
            dayButton.SetInteractable(false);
            dayButton.SetCalendar(this);
            dayButton.SetColor(_otherMonthColor);
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);

            if (new DateTime(nextDate.Year, nextDate.Month, i + 1).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetColor(_weekendColor);
            }

            _dayButtons.Add(dayButton);
        }
    }



    private void GenerateMonthList()
    {
        for (int i = 0; i < 5; i++)
        {
            DateTime date = _date.AddMonths(-1);

            if (i == 0)
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date.Month, GetShortMonthName(date.Month), this);
                _monthToggles.Add(m);
            }
            else
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date.AddMonths(i).Month, GetShortMonthName(date.AddMonths(i).Month), this);
                if(date.AddMonths(i).Month == _month)
                {
                    m.SelectMonth();
                }
                _monthToggles.Add(m);
            }
        }
    }



    public string GetShortMonthName(int month)
    {
        print(month);
        return _months.ContainsKey(month) ? _months[month][1] : "none";
    }



    public string GetFullMonthName(int month)
    {
        return _months.ContainsKey(month) ? _months[month][0] : "none";
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
