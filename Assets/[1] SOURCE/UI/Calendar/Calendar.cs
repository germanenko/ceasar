using Doozy.Runtime.UIManager.Components;
using Germanenko.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [SerializeField] private int _day;

    [SerializeField] private int _previousCharCount;

    [SerializeField] private DayButton _dayButtonPrefab;
    [SerializeField] private MonthToggle _monthTogglePrefab;

    [SerializeField] private Transform _dayButtonsParent;
    [SerializeField] private Transform _monthTogglesParent;

    [SerializeField] private List<DayButton> _dayButtons;
    [SerializeField] private List<MonthToggle> _monthToggles;

    [SerializeField] private TMP_InputField _dateText;
    [SerializeField] private TextMeshProUGUI _dateTextInButton;

    [SerializeField] private DateTime _startDate;
    [SerializeField] private DateTime _date;

    [SerializeField] private GameObject _calendarWindow;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _weekendColor;
    [SerializeField] private Color _otherMonthColor;

    [SerializeField] private UIToggleGroup _daysToggleGroup;
    [SerializeField] private UIToggleGroup _monthsToggleGroup;

    [SerializeField] private GridLayoutGroup _daysGrid;

    private void Start()
    {
        _day = DateTime.Now.Day;
        _year = DateTime.Now.Year;

        _date = DateTime.Now;
        _startDate = _date;

        _dateTextInButton.text = _date.Date.ToShortDateString();

        GenerateMonthList();

    }



    private void Update()
    {
        if(Input.touchCount > 0)
        {
            CheckIfOutside();
        }
    }



    public void OpenCalendarWindow()
    {
        _calendarWindow.SetActive(true);

        _daysGrid.cellSize = new Vector2(_daysGrid.GetComponent<RectTransform>().rect.width / 7, _daysGrid.GetComponent<RectTransform>().rect.height / 6 - 3);

        GenerateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }



    public void CloseCalendarWindow()
    {
        _calendarWindow.SetActive(false);
    }



    public void SetYear(float year)
    {
        _year = (int)year;
        UpdateDateText();
    }



    public void SetMonth(DateTime month)
    {
        _date = new DateTime(month.Year, month.Month, _date.Day);
        UpdateDateText();
        GenerateCalendar(_date.Year, _date.Month);
    }



    public void SetDay(DateTime date)
    {
        //_date = new DateTime(_date.Year, _date.Month, day);
        _date = date;
        UpdateDateText();
    }



    private void UpdateDateText()
    {
        if(Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            _dateText.text = _date.ToString("dd.MM.yy");
        }
        else if(Localization.Instance.Language == LocalizationLanguage.USA)
        {
            _dateText.text = _date.ToString("MM.dd.yy");
        }
    }



    private void GenerateCalendar(int year, int month)
    {
        //if (_dayButtons.Count > 0) return;

        foreach (var item in _dayButtons)
        {
            item.Drop();

            //Pooler.Instance.Despawn(PoolType.Entities, item.gameObject);
        }

        //_dayButtons.Clear();

        if(_dayButtons.Count > 0)
        {
            DateTime prevDate = _date;
            prevDate = prevDate.AddMonths(-1);

            int j = 0;

            for (int i = 0; i < (Localization.Instance.Language == LocalizationLanguage.Russia ? GetMonthStartDay(year, month) : GetMonthStartDay(year, month) + 1); i++) // ����� �������� ������
            {
                DayButton dayButton = _dayButtons[j];
                j++;
                DateTime dt;

                if (Localization.Instance.Language == LocalizationLanguage.Russia)
                    dt = new DateTime(prevDate.Year, prevDate.Month, DateTime.DaysInMonth(prevDate.Year, prevDate.Month) - GetMonthStartDay(year, month) + i + 1);
                else
                    dt = new DateTime(prevDate.Year, prevDate.Month, DateTime.DaysInMonth(prevDate.Year, prevDate.Month) - GetMonthStartDay(year, month) + i);

                dayButton.SetDay(dt); //DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1
                dayButton.SetCalendar(this);
                dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
                dayButton.SetNextAndPreviousMonth(false, true);

                if (Localization.Instance.Language == LocalizationLanguage.Russia)
                {
                    if (new DateTime(prevDate.Year, prevDate.Month, DateTime.DaysInMonth(prevDate.Year, prevDate.Month) - GetMonthStartDay(year, month) + i + 1).DayOfWeek == DayOfWeek.Sunday)
                    {
                        dayButton.SetWeekend(true);
                    }
                }
                else
                {
                    if (new DateTime(prevDate.Year, prevDate.Month, DateTime.DaysInMonth(prevDate.Year, prevDate.Month) - GetMonthStartDay(year, month) + i).DayOfWeek == DayOfWeek.Sunday)
                    {
                        dayButton.SetWeekend(true);
                    }
                }
            }



            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++) // ����� �������� ������
            {
                DayButton dayButton = _dayButtons[j];
                j++;
                DateTime dt = new DateTime(year, month, i + 1);

                dayButton.SetDay(dt);
                dayButton.SetCalendar(this);
                dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
                dayButton.SetNextAndPreviousMonth(false, false);

                if (new DateTime(year, month, i + 1).DayOfWeek == DayOfWeek.Sunday)
                {
                    dayButton.SetWeekend(true);
                }

                if (i + 1 == _day && _date.Month == DateTime.Now.Month)
                {
                    dayButton.UIToggle.isOn = true;
                    dayButton.DayText.fontStyle = FontStyles.Bold;
                }
            }

            int lDays = 42 - j;

            DateTime nDate = _date;
            nDate = nDate.AddMonths(1);

            for (int i = 0; i < lDays; i++) // ����� ���������� ������
            {
                DayButton dayButton = _dayButtons[j];
                j++;
                DateTime dt = new DateTime(nDate.Year, nDate.Month, i + 1);

                dayButton.SetDay(dt);
                dayButton.SetCalendar(this);
                dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
                dayButton.SetNextAndPreviousMonth(true, false);

                if (new DateTime(nDate.Year, nDate.Month, i + 1).DayOfWeek == DayOfWeek.Sunday)
                {
                    dayButton.SetWeekend(true);
                }
            }
            return;
        }

        #region GenerateButtons
        DateTime previousDate = _date;
        previousDate = previousDate.AddMonths(-1);

        for (int i = 0; i < (Localization.Instance.Language == LocalizationLanguage.Russia ? GetMonthStartDay(year, month) : GetMonthStartDay(year, month) + 1); i++) // ����� �������� ������
        {
            DayButton dayButton = SpawnDayButton();

            DateTime dt;

            if(Localization.Instance.Language == LocalizationLanguage.Russia)
                dt = new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(previousDate.Year, previousDate.Month) - GetMonthStartDay(year, month) + i + 1);
            else
                dt = new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(previousDate.Year, previousDate.Month) - GetMonthStartDay(year, month) + i);

            dayButton.SetDay(dt); //DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1
            dayButton.SetCalendar(this);
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
            dayButton.SetNextAndPreviousMonth(false, true);

            if (Localization.Instance.Language == LocalizationLanguage.Russia)
            {
                if (new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(previousDate.Year, previousDate.Month) - GetMonthStartDay(year, month) + i + 1).DayOfWeek == DayOfWeek.Sunday)
                {
                    dayButton.SetWeekend(true);
                }
            }
            else
            {
                if (new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(previousDate.Year, previousDate.Month) - GetMonthStartDay(year, month) + i).DayOfWeek == DayOfWeek.Sunday)
                {
                    dayButton.SetWeekend(true);
                }
            }    

            _dayButtons.Add(dayButton);
        }



        for (int i = 0; i < DateTime.DaysInMonth(year, month); i++) // ����� �������� ������
        {
            DayButton dayButton = SpawnDayButton();

            DateTime dt = new DateTime(year, month, i+1);

            dayButton.SetDay(dt);
            dayButton.SetCalendar(this);
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
            dayButton.SetNextAndPreviousMonth(false, false);

            if (new DateTime(year, month, i + 1).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetWeekend(true);
            }

            if(i+1 == _day && _date.Month == DateTime.Now.Month)
            {
                dayButton.UIToggle.isOn = true;
                dayButton.DayText.fontStyle = FontStyles.Bold;
            }

            _dayButtons.Add(dayButton);
        }

        int lastDays = 42 - _dayButtons.Count;

        DateTime nextDate = _date;
        nextDate = nextDate.AddMonths(1);

        for (int i = 0; i < lastDays; i++) // ����� ���������� ������
        {
            DayButton dayButton = SpawnDayButton();

            DateTime dt = new DateTime(nextDate.Year, nextDate.Month, i + 1);

            dayButton.SetDay(dt);
            dayButton.SetCalendar(this);
            dayButton.UIToggle.AddToToggleGroup(_daysToggleGroup);
            dayButton.SetNextAndPreviousMonth(true, false);

            if (new DateTime(nextDate.Year, nextDate.Month, i + 1).DayOfWeek == DayOfWeek.Sunday)
            {
                dayButton.SetWeekend(true);
            }

            _dayButtons.Add(dayButton);
        }
        #endregion
    }



    private void GenerateMonthList()
    {
        for (int i = 0; i < 5; i++)
        {
            DateTime date = _date.AddMonths(-1);

            if (i == 0)
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date, GetShortMonthName(date.Month) + (date.Year != _date.Year ? $" {date.ToString("yy")}" : ""), this);
                _monthToggles.Add(m);

                m.UIToggle.AddToToggleGroup(_monthsToggleGroup);
            }
            else
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date.AddMonths(i), GetShortMonthName(date.AddMonths(i).Month) + (date.AddMonths(i).Year != _date.Year ? $" {date.AddMonths(i).ToString("yy")}" : ""), this);
                if(date.AddMonths(i).Month == _date.Month)
                {
                    m.SelectMonth();
                }
                _monthToggles.Add(m);

                m.UIToggle.AddToToggleGroup(_monthsToggleGroup);
            }
        }
    }



    public string GetShortMonthName(int month)
    {
        return _months.ContainsKey(month) ? _months[month][1] : "none";
    }



    public string GetFullMonthName(int month)
    {
        return _months.ContainsKey(month) ? _months[month][0] : "none";
    }



    public void GetDateFromInputField()
    {
        string[] values = _dateText.text.Split('.');

        try
        {
            DateTime d = new DateTime(int.Parse(values[2]), int.Parse(values[1]), int.Parse(values[0]));
            _date = d;
            _dateText.text = _date.ToShortDateString();
        }
        catch (Exception e)
        {
            _dateText.text = _date.ToShortDateString();
        }
    }



    public void ConfirmDate()
    {
        _dateTextInButton.text = _date.ToShortDateString();
        CloseCalendarWindow();
    }



    public void DateCorrect()
    {
        int currentCharCount = _dateText.text.Length;

        if (currentCharCount < _previousCharCount)
        {
            _previousCharCount = _dateText.text.Length; 
            return;
        }

        switch (_dateText.text.Length)
        {
            case 2:
                _dateText.text += ".";
                _dateText.MoveToEndOfLine(false, false);
                break;
            case 5:
                _dateText.text += ".";
                _dateText.MoveToEndOfLine(false, false);
                break;
        }

        _previousCharCount = _dateText.text.Length;
    }



    private DayButton SpawnDayButton()
    {
        var d = Pooler.Instance.Spawn(PoolType.Entities, _dayButtonPrefab.gameObject, default, default, _dayButtonsParent);
        DayButton dayButton = d.GetComponent<DayButton>();
        return dayButton;
    }



    private void CheckIfOutside()
    {
        Camera camera = null;
        if (!RectTransformUtility.RectangleContainsScreenPoint(_calendarWindow.GetComponent<RectTransform>(), Input.mousePosition, camera))
            CloseCalendarWindow();
    }



    private int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        return (int)temp.DayOfWeek - 1;
    }
}
