using DG.Tweening;
using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using Germanenko.Framework;
using Germanenko.Source;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clock : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TimeLabel _timeLabelHours;
    [SerializeField] private TimeLabel _timeLabelMinutes;

    [SerializeField] private bool _isStartTime;
    [SerializeField] private bool _correctPeriod;

    [SerializeField] private TextMeshProUGUI _startTimeText;
    [SerializeField] private TextMeshProUGUI _endTimeText;

    [SerializeField] List<TimeLabel> _hourList;
    [SerializeField] List<TimeLabel> _minuteList;

    [SerializeField] private Color _startTimeColor;
    [SerializeField] private Color _endTimeColor;
    [SerializeField] private Color _incorrectPeriodColor;

    [SerializeField] private Transform _clock;
    [SerializeField] private GameObject _timeTypeSelector;
    [SerializeField] private TextMeshProUGUI _timeButtonText;

    [SerializeField] private bool _isDay;

    [SerializeField] private DateTime _startTime;
    public DateTime StartTime => _startTime;

    [SerializeField] private DateTime _endTime;
    public DateTime EndTime => _endTime;

    [SerializeField] private PlayMakerFSM _dayToggle;
    [SerializeField] private PlayMakerFSM _nightToggle;

    void Start()
    {
        DrawHour(12, 140);
        DrawMinute(60, 230);
    }



    private void Update()
    {
        if (Input.touchCount > 0)
        {
            CheckIfOutside();
        }
    }



    private void CheckIfOutside()
    {
        Camera camera = null;
        if (!RectTransformUtility.RectangleContainsScreenPoint(_clock.GetComponent<RectTransform>(), Input.mousePosition, camera))
            SetActiveTimeSelector(false);
    }



    public void SetActiveTimeSelector(bool activate)
    {

        if (!_correctPeriod) return;

        _clock.gameObject.SetActive(activate);

        _timeTypeSelector.SetActive(activate);
        _timeButtonText.gameObject.SetActive(!activate);

        _startTimeText.GetComponent<UIToggle>().isOn = true;

        //transform.parent.parent.GetComponent<ScrollRect>().vertical = !activate;

        if (_hourList.Count > 0)
        {
            RegenerateHours();
        }

        if (activate)
        {

            if (_startTime.Hour <= 12)
            {
                _nightToggle.SendEvent("SetIsOn");
                print("isNight");
            }
            else
            {
                _dayToggle.SendEvent("SetIsOn");
                print("isDay");
            }
        }
        

        if (Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            if(_startTime.Hour == _endTime.Hour && _startTime.Minute == _endTime.Minute)    
            {
                SetPreviewPeriodText(string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute));
            }
            else
            {
                SetPreviewPeriodText($"{string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute)} - {string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute)}");
            }
            
        }
        else if (Localization.Instance.Language == LocalizationLanguage.USA)
        {
            if (_startTime.Hour == _endTime.Hour && _startTime.Minute == _endTime.Minute)
            {
                SetPreviewPeriodText(_startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture));
            }
            else
            {
                SetPreviewPeriodText($"{_startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)} - {_endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)}");
            }
        }
        
        if (Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            _startTimeText.text = string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute);
            _endTimeText.text = string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute);
        }
        else if (Localization.Instance.Language == LocalizationLanguage.USA)
        {
            _startTimeText.text = $"{_startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)}";
            _endTimeText.text = $"{_endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)}";
        }

    }



    public void ChangePeriod()
    {

        if (_isStartTime)
        {
            _endTimeText.GetComponent<UIToggle>().isOn = true;
        }
        else
        {
            SetActiveTimeSelector(false);
        }

    }



    private void CheckCorrectPeriod()
    {
        if(Localization.Instance.Language == LocalizationLanguage.USA)
        {
            if(_startTime > _endTime)
            {
                _correctPeriod = false;
                _endTimeText.color = _incorrectPeriodColor;

                if (!_isStartTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endTime.Hour)
                            label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endTime.Minute)
                            label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                    }
                }
            }
            else
            {
                _correctPeriod = true;

                _endTimeText.color = _endTimeColor;


                if (!_isStartTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endTime.Hour)
                            label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endTime.Minute)
                            label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    }
                }
            }
            return;
        }

        if (_endTime.Hour == 0 && _endTime.Minute == 0)
        {
            _correctPeriod = true;
            return;
        }


        if (_endTime.Hour < _startTime.Hour)
        {

            _correctPeriod = false;
            _endTimeText.color = _incorrectPeriodColor;



            if (!_isStartTime)
            {
                foreach (TimeLabel label in _hourList)
                {
                    if (label.TimeValue == _endTime.Hour)
                        label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                }
                foreach (TimeLabel label in _minuteList)
                {
                    if (label.TimeValue == _endTime.Minute)
                        label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                }
            }

        }

        else
        {
            if (_endTime.Hour == _startTime.Hour && _endTime.Minute < _startTime.Minute)
            {

                _correctPeriod = false;
                _endTimeText.color = _incorrectPeriodColor;


                if (!_isStartTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endTime.Hour)
                            label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endTime.Minute)
                            label.SelectTime(true, _isStartTime, _incorrectPeriodColor);
                    }
                }

            }
            else
            {

                _correctPeriod = true;
                _endTimeText.color = _endTimeColor;


                if (!_isStartTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endTime.Hour)
                            label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endTime.Minute)
                            label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    }
                }

            }
        }

    }



    public void SetTime(int time, TimeLabelType type, bool update = false)
    {
        switch (type)
        {
            case TimeLabelType.Hour:
                foreach(TimeLabel label in _hourList)
                {
                    if(label.TimeValue == time)
                    {
                        if (update) break;
                        label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    }
                    else
                        if(!label.Selected || (label.Selected && label.IsStartTime == _isStartTime))
                            label.SelectTime(false);
                }

                if (_isStartTime)
                {
                    _startTime = _startTime.Date + new TimeSpan(time, _startTime.Minute, 0);
                }
                else
                {
                    _endTime = _endTime.Date + new TimeSpan(time, _endTime.Minute, 0);
                }
                break;

            case TimeLabelType.Minute:

                foreach (TimeLabel label in _minuteList)
                {
                    if (label.TimeValue == time)
                        label.SelectTime(true, _isStartTime, _isStartTime ? _startTimeColor : _endTimeColor);
                    else
                        if (!label.Selected || (label.Selected && label.IsStartTime == _isStartTime))
                        label.SelectTime(false);
                }

                if (_isStartTime)
                    _startTime = _startTime.Date + new TimeSpan(_startTime.Hour, time, 0);
                else
                    _endTime = _endTime.Date + new TimeSpan(_endTime.Hour, time, 0);



                //if (Localization.Instance.Language == LocalizationLanguage.Russia)
                //{
                //    if (_isStartTime)
                //        _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
                //    else
                //        _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);
                //}
                //if (Localization.Instance.Language == LocalizationLanguage.USA)
                //{
                //    if (_isStartTime)
                //        _startTimeText.text = _startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                //    else
                //        _endTimeText.text = _endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                //}

                break;
        }

        UpdateTime();

        CheckCorrectPeriod();

        if (Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            if (_isStartTime)
                _startTimeText.text = string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute);
            else
                _endTimeText.text = string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute);
        }
        else if(Localization.Instance.Language == LocalizationLanguage.USA)
        {
            if (_isStartTime)
                _startTimeText.text = _startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            else
                _endTimeText.text = _endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }
    }



    public void SelectTimeType(bool startTime)
    {
        _isStartTime = startTime;

        ClearClockMarks();
        EnableMarks();

        if (_isStartTime)
        {
            _startTimeText.fontStyle = FontStyles.Bold;
            _endTimeText.fontStyle = FontStyles.Normal;
        }
        else
        {
            _startTimeText.fontStyle = FontStyles.Normal;
            _endTimeText.fontStyle = FontStyles.Bold;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_startTimeText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_endTimeText.rectTransform);
    }



    public DateTime GetStartPeriod()
    {
        return new DateTime(1, 1, 1, _startTime.Hour, _startTime.Minute, 0);
    }



    public DateTime GetEndPeriod()
    {
        return new DateTime(1, 1, 1, _endTime.Hour, _endTime.Minute, 0);
    }



    public void SetPeriod(DateTime start, DateTime end)
    {
        ClearClockMarks();

        _startTime = start;
        _endTime = end;

        _startTimeText.text = string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute);
        _endTimeText.text = string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute);

        EnableMarks();
    }



    public void SetPreviewPeriodText(string text)
    {
        _timeButtonText.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_timeButtonText.rectTransform);
    }



    public void EnableMarks()
    {
        if (_isStartTime)
        {
            foreach (TimeLabel label in _hourList)
            {
                if (label.TimeValue == _startTime.Hour)
                    label.SelectTime(true, true, _startTimeColor);

            }

            foreach (TimeLabel label in _minuteList)
            {
                if (label.TimeValue == _startTime.Minute)
                    label.SelectTime(true, true, _startTimeColor);

            }
        }
        else
        {
            foreach (TimeLabel label in _hourList)
            {

                if (label.TimeValue == _endTime.Hour)
                    label.SelectTime(true, false, _endTimeColor);
            }

            foreach (TimeLabel label in _minuteList)
            {

                if (label.TimeValue == _endTime.Minute)
                    label.SelectTime(true, false, _endTimeColor);
            }
        }
    }



    public void ClearClockMarks()
    {
        foreach (TimeLabel label in _hourList)
        {
            label.SelectTime(false);
        }
        foreach (TimeLabel label in _minuteList)
        {
            label.SelectTime(false);
        }
    }



    public void ClearTime()
    {
        _startTime = DateTime.Now;
        _endTime = DateTime.Now;

        _startTimeText.text = string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute);
        _endTimeText.text = string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute);

        ClearClockMarks();
    }



    public GameObject GetClockGameObject()
    {
        return _clock.gameObject;
    }



    public bool GetStartTime()
    {
        return _isStartTime;
    }



    private void UpdateTime()
    {
        if(Localization.Instance.Language == LocalizationLanguage.USA)
        {
            if (_isStartTime)
            {
                _startTime = DateTime.ParseExact(string.Format("{0:00}:{1:00}", _startTime.Hour, _startTime.Minute) + " " + (_isDay ? "AM" : "PM"), "hh:mm tt", CultureInfo.InvariantCulture);
            }
            else
            {
                _endTime = DateTime.ParseExact(string.Format("{0:00}:{1:00}", _endTime.Hour, _endTime.Minute) + " " + (_isDay ? "AM" : "PM"), "hh:mm tt", CultureInfo.InvariantCulture);
            }
        }
    }

    

    #region ClockGenerator

    public void ChangeDayTimeHours(bool isDay)
    {
        _isDay = isDay;

        if (Localization.Instance.Language == LocalizationLanguage.USA)
        {
            for (int i = 0; i < _hourList.Count; i++)
            {
                if (_isDay)
                {
                    //_hourList[i].LabelText.text = "12";
                    //_hourList[i].TimeValue = 12;

                    _hourList[i].LerpValueAmerican(true, .5f);
                }
                else
                {
                    //_hourList[i].LabelText.text = i.ToString();
                    //_hourList[i].TimeValue = i;
                    _hourList[i].LerpValueAmerican(false, .5f);
                }
            }

            foreach (var hour in _hourList)
            {
                if (hour.Selected)
                {
                    hour.SetTime();
                }
            }
            
            return;
        }

        for (int i = 0; i < _hourList.Count; i++)
        {
            if (i == 0)
            {
                if (_isDay)
                {
                    //_hourList[i].LabelText.text = "12";
                    //_hourList[i].TimeValue = 12;

                    _hourList[i].LerpValue(_hourList[i].TimeValue, 12, .5f);
                }
                else
                {
                    //_hourList[i].LabelText.text = i.ToString();
                    //_hourList[i].TimeValue = i;
                    _hourList[i].LerpValue(_hourList[i].TimeValue, i, .5f);
                }
            }
            else
            {
                if (_isDay)
                {
                    //_hourList[i].LabelText.text = (i + 12).ToString();
                    //_hourList[i].TimeValue = i + 12;
                    _hourList[i].LerpValue(_hourList[i].TimeValue, i + 12, .5f);
                }
                else
                {
                    //_hourList[i].LabelText.text = i.ToString();
                    //_hourList[i].TimeValue = i;
                    _hourList[i].LerpValue(_hourList[i].TimeValue, i, .5f);
                }
            }
        }

        foreach (var hour in _hourList)
        {
            if (hour.Selected)
            {
                hour.SetTime();
            }
        }


        //_hourList.Clear();

        //DrawHour(12, 140);
    }



    public void RegenerateHours()
    {
        if (Localization.Instance.Language == LocalizationLanguage.USA)
        {
            for (int i = 0; i < _hourList.Count; i++)
            {
                if(i == 0)
                {
                    _hourList[i].TimeValue = 12;
                }
                else
                {
                    _hourList[i].TimeValue = i;                   
                }
                _hourList[i].LabelText.text = _hourList[i].TimeValue.ToString();
            }
            return;
        }
        else if(Localization.Instance.Language == LocalizationLanguage.Russia)
        {
            for (int i = 0; i < _hourList.Count; i++)
            {
                if (i == 0)
                {
                    if (_isDay)
                    {
                        _hourList[i].LabelText.text = "12";
                        _hourList[i].TimeValue = 12;
                    }
                    else
                    {
                        _hourList[i].LabelText.text = i.ToString();
                        _hourList[i].TimeValue = i;
                    }
                    _hourList[i].OnEndIncrement.AddListener(EnableMarks);
                }
                else
                {
                    if (_isDay)
                    {
                        _hourList[i].LabelText.text = (i + 12).ToString();
                        _hourList[i].TimeValue = i + 12;
                    }
                    else
                    {
                        _hourList[i].LabelText.text = i.ToString();
                        _hourList[i].TimeValue = i;
                    }
                }
            }
            
        }

        foreach (var hour in _hourList)
        {
            if (hour.Selected)
            {
                hour.SetTime();
            }
        }
    }



    private void DrawHour(int steps, int radius)
    {
        for (int i = 0; i < steps; i++)
        {
            float circumferenceProgress = (float)i / steps;

            float currectRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Sin(currectRadian);
            float yScaled = Mathf.Cos(currectRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;



            var h = Pooler.Instance.Spawn(PoolType.Entities, _timeLabelHours.gameObject, default, default, _clock);

            h.transform.localPosition = new Vector3(x, y, 0);

            var s = h.GetComponent<TimeLabel>();

            //s.transform.localScale = s.transform.localScale * 1.5f;

            s.LabelType = TimeLabelType.Hour;
            s.Clock = this;


            if(Localization.Instance.Language == LocalizationLanguage.USA)
            {
                if (i == 0)
                {
                    s.TimeValue = 12;
                    s.LabelText.text = "12";
                    s.OnEndIncrement.AddListener(EnableMarks);
                }
                else
                {
                    s.LabelText.text = i.ToString();
                    s.TimeValue = i;
                }
            }
            else if (Localization.Instance.Language == LocalizationLanguage.Russia)
            {
                if (i == 0)
                {
                    if (_isDay)
                    {
                        s.LabelText.text = "12";
                        s.TimeValue = 12;
                    }
                    else
                    {
                        s.LabelText.text = i.ToString();
                        s.TimeValue = i;
                    }
                    s.OnEndIncrement.AddListener(EnableMarks);
                }
                else
                {
                    if (_isDay)
                    {
                        s.LabelText.text = (i + 12).ToString();
                        s.TimeValue = i + 12;
                    }
                    else
                    {
                        s.LabelText.text = i.ToString();
                        s.TimeValue = i;
                    }
                }
            }

            _hourList.Add(s);
        }
    }



    private void DrawMinute(int steps, int radius)
    {
        for (int i = 0; i < steps; i++)
        {
            float circumferenceProgress = (float)i / steps;

            float currectRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Sin(currectRadian);
            float yScaled = Mathf.Cos(currectRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;



            var s = Instantiate(_timeLabelMinutes, _clock);

            s.LabelType = TimeLabelType.Minute;

            s.transform.localPosition = new Vector3(x, y, 0);

            s.TimeValue = i;
            s.Clock = this;



            if (i % 5 == 0)
            {
                s.LabelText.text = i.ToString();
            }
            else
            {
                s.LabelText.text = "";
            }

            _minuteList.Add(s);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("pointerDown");
    }

    #endregion

}