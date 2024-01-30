using Doozy.Runtime.UIManager.Animators;
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
    [SerializeField] private Transform _calendarButton;

    [SerializeField] private List<DayButton> _dayButtons;
    [SerializeField] private List<MonthToggle> _monthToggles;

    [SerializeField] private TMP_InputField _dateText;
    [SerializeField] private TextMeshProUGUI _dateTextInButton;
    [SerializeField] private TextMeshProUGUI _startDateText;
    [SerializeField] private TextMeshProUGUI _endDateText;

    [SerializeField] private DateTime _startDate;
    [SerializeField] private DateTime _endDate;
    [SerializeField] private DateTime _date;

    [SerializeField] private GameObject _calendarWindow;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _weekendColor;
    [SerializeField] private Color _otherMonthColor;

    [SerializeField] private Color _endDateColor;
    [SerializeField] private Color _incorrectPeriodColor;

    [SerializeField] private UIToggleGroup _daysToggleGroup;
    [SerializeField] private UIToggleGroup _monthsToggleGroup;

    [SerializeField] private GridLayoutGroup _daysGrid;

    [SerializeField] private Transform _currentMonthToggle;

    [SerializeField] private bool _isStartDate = true;
    [SerializeField] private bool _calendarIsOpened;
    [SerializeField] private bool _correctPeriod = true;

    public UnityEvent<bool> OnDateSelected; // событие при выборе даты, возвращающее, период это или конкретный день

    private void Start()
    {
        _day = DateTime.Now.Day;
        _year = DateTime.Now.Year;

        _date = DateTime.Now;
        _startDate = _date;
        _endDate = _date;

        UpdateDateText();

        GenerateMonthList();

    }



    private void Update()
    {
        if(Input.touchCount > 0 && _calendarIsOpened)
        {
            CheckIfOutside();
        }
    }



    public void OpenCalendarWindow()
    {
        _calendarWindow.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_monthTogglesParent.GetComponent<RectTransform>());

        float distanceBtwnTouchAndCurrentMonth = _calendarButton.position.y - _currentMonthToggle.position.y;
        _calendarWindow.transform.position = new Vector3(_calendarWindow.transform.position.x, _calendarWindow.transform.position.y - -distanceBtwnTouchAndCurrentMonth, 0);

        _daysGrid.cellSize = new Vector2(_daysGrid.GetComponent<RectTransform>().rect.width / 7, _daysGrid.GetComponent<RectTransform>().rect.height / 6 - 3);

        GenerateCalendar(DateTime.Now.Year, DateTime.Now.Month);

        _calendarIsOpened = true;
    }



    public void CloseCalendarWindow(bool saveDate)
    {
        if(saveDate)
        {
            ConfirmDate();
        }

        if (!_correctPeriod) return;

        if (_isStartDate)
        { 
            ChangePeriod();
            return;
        }

        foreach (var item in _dayButtons)
        {
            item.Drop();
        }

        _calendarWindow.SetActive(false);
        _calendarIsOpened = false;
        _startDateText.GetComponent<UIToggle>().isOn = true;

        print($"{_startDate.Date.ToShortDateString()}  {_endDate.Date.ToShortDateString()}");

        OnDateSelected?.Invoke(_startDate.Date != _endDate.Date);
    }



    public void ChangePeriod()
    {
        print("changePeriod");

        if (_isStartDate)
        {
            _endDateText.GetComponent<UIToggle>().isOn = true;
        }
        else
        {
            CloseCalendarWindow(false);
        }

    }



    public void SelectTimeType(bool startTime)
    {
        _isStartDate = startTime;

        if (_isStartDate)
        {
            _startDateText.fontStyle = FontStyles.Bold;
            _endDateText.fontStyle = FontStyles.Normal;
        }
        else
        {
            _startDateText.fontStyle = FontStyles.Normal;
            _endDateText.fontStyle = FontStyles.Bold;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_startDateText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_endDateText.rectTransform);
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

        if (_isStartDate)
        {
            _startDate = date;
            if (Localization.Instance.Language == LocalizationLanguage.Russia)
            {
                _startDateText.text = _date.ToString("dd.MM.yy");
            }
            else if (Localization.Instance.Language == LocalizationLanguage.USA)
            {
                _startDateText.text = _date.ToString("MM.dd.yy");
            }
        }
        else
        {
            _endDate = date;
            if (Localization.Instance.Language == LocalizationLanguage.Russia)
            {
                _endDateText.text = _date.ToString("dd.MM.yy");
            }
            else if (Localization.Instance.Language == LocalizationLanguage.USA)
            {
                _endDateText.text = _date.ToString("MM.dd.yy");
            }
        }

        CheckCorrectPeriod(); 
    }



    public void CheckCorrectPeriod()
    {
        if(_startDate.Date > _endDate.Date)
        {
            print("incorrect");
            _endDateText.color = _incorrectPeriodColor;
            _correctPeriod = false;
        }
        else
        {
            print("correct");
            _endDateText.color = _endDateColor;
            _correctPeriod = true;
        }
    }



    private void UpdateDateText()
    {
        if(Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            _dateText.text = _date.ToString("dd.MM.yy");
            _startDateText.text = _startDate.ToString("dd.MM.yy");
            _endDateText.text = _endDate.ToString("dd.MM.yy");

            if (_startDate.Day == _endDate.Day && _startDate.Month == _endDate.Month)
            {
                _dateTextInButton.text = _date.ToString("dd.MM.yy");
            }
            else
            {
                _dateTextInButton.text = $"{_startDate.ToString("dd.MM.yy")} - {_endDate.ToString("dd.MM.yy")}";
            }
        }
        else if(Localization.Instance.Language == LocalizationLanguage.USA)
        {
            _dateText.text = _date.ToString("MM.dd.yy");
            _startDateText.text = _startDate.ToString("MM.dd.yy");
            _endDateText.text = _endDate.ToString("MM.dd.yy");

            if(_startDate.Day == _endDate.Day && _startDate.Month == _endDate.Month)
            {
                _dateTextInButton.text = _date.ToString("MM.dd.yy");
            }
            else
            {
                _dateTextInButton.text = $"{_startDate.ToString("MM.dd.yy")} - {_endDate.ToString("MM.dd.yy")}";
            }
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

            for (int i = 0; i < (Localization.Instance.Language == LocalizationLanguage.Russia ? GetMonthStartDay(year, month) : GetMonthStartDay(year, month) + 1); i++) // числа прошлого месяца
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



            for (int i = 0; i < DateTime.DaysInMonth(year, month); i++) // числа текущего месяца
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

                if (i + 1 == _date.Day)
                {
                    dayButton.UIToggle.isOn = true;
                }

                if (i + 1 == _date.Day && _date.Month == DateTime.Now.Month)
                {
                    dayButton.DayText.fontStyle = FontStyles.Bold;
                }
            }

            int lDays = 42 - j;

            DateTime nDate = _date;
            nDate = nDate.AddMonths(1);

            for (int i = 0; i < lDays; i++) // числа следующего месяца
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

        for (int i = 0; i < (Localization.Instance.Language == LocalizationLanguage.Russia ? GetMonthStartDay(year, month) : GetMonthStartDay(year, month) + 1); i++) // числа прошлого месяца
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



        for (int i = 0; i < DateTime.DaysInMonth(year, month); i++) // числа текущего месяца
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

        for (int i = 0; i < lastDays; i++) // числа следующего месяца
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
        foreach (var monthToggle in _monthToggles)
        {
            Destroy(monthToggle.gameObject);
        }

        _monthToggles.Clear();

        for (int i = 0; i < 5; i++)
        {
            DateTime date = _date.AddMonths(-1);

            if (i == 0)
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date, GetShortMonthName(date.Month) + (date.Year != DateTime.Now.Year ? $" {date.ToString("yy")}" : ""), this);
                _monthToggles.Add(m);

                m.UIToggle.AddToToggleGroup(_monthsToggleGroup);
            }
            else
            {
                var m = Pooler.Instance.Spawn(PoolType.Entities, _monthTogglePrefab.gameObject, default, default, _monthTogglesParent).GetComponent<MonthToggle>();
                m.Init(date.AddMonths(i), GetShortMonthName(date.AddMonths(i).Month) + (date.AddMonths(i).Year != DateTime.Now.Year ? $" {date.AddMonths(i).ToString("yy")}" : ""), this);

                if(date.AddMonths(i).Month == _date.Month)
                {
                    m.UIToggle.isOn = true;

                    _currentMonthToggle = m.transform;
                }

                if(date.AddMonths(i).Month == DateTime.Now.Month)
                {
                    m.SetBoldOnMonthName();
                }

                _monthToggles.Add(m);

                m.UIToggle.AddToToggleGroup(_monthsToggleGroup);
            }
        }

    }



    public void SetIsStartDate(bool isStart)
    {
        _isStartDate = isStart;
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
            int year;
            if (values[2].Length < 4)
            {
                if (int.Parse(values[2]) > 30 && int.Parse(values[2]) < 100)
                {
                    year = 1900 + int.Parse(values[2]);
                }
                else
                {
                    year = 2000 + int.Parse(values[2]);
                }
            }
            else
                year = int.Parse(values[2]);

            DateTime d = new DateTime(year, int.Parse(values[1]), int.Parse(values[0]));
            _date = d;
            print($"{_date.Day}.{_date.Month}.{_date.Year}");
            UpdateDateText();

            GenerateMonthList();
            GenerateCalendar(_date.Year, _date.Month);

        }
        catch (Exception e)
        {
            print(e.Message);
            UpdateDateText();
        }
    }



    public void ConfirmDate()
    {
        if (_isStartDate)
        {
            _startDate = _date;
            _endDate = _date;
        }
        else
        {
            _endDate = _date;
        }

        UpdateDateText();
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
            CloseCalendarWindow(false);
    }



    private int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        return (int)temp.DayOfWeek - 1;
    }
}
