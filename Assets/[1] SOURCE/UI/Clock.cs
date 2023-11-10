using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private TimeLabel _timeLabel;

    [SerializeField] private bool _startTime;
    [SerializeField] private bool _correctPeriod;

    [SerializeField] private TextMeshProUGUI _startTimeText;
    [SerializeField] private TextMeshProUGUI _endTimeText;

    [SerializeField] private int _startHours;
    [SerializeField] private int _startMinutes;

    [SerializeField] private int _endHours;
    [SerializeField] private int _endMinutes;

    [SerializeField] List<TimeLabel> _hourList;
    [SerializeField] List<TimeLabel> _minuteList;

    [SerializeField] private Color _startTimeColor;
    [SerializeField] private Color _endTimeColor;
    [SerializeField] private Color _incorrectPeriodColor;

    [SerializeField] private Transform _clock;
    [SerializeField] private GameObject _timeTypeSelector;
    [SerializeField] private TextMeshProUGUI _timeButtonText;

    [SerializeField] private UISelectable _clockSelectable;

    void Start()
    {
        DrawHour(12, 120);
        DrawMinute(60, 230);
    }



    public void SetActiveTimeSelector(bool activate)
    {
        if (!_correctPeriod) return;

        _clock.gameObject.SetActive(activate);

        if (activate)
        {
            _clockSelectable.Select();
        }
        else
        {
            if (_startTime)
            {
                _endHours = _startHours;
                _endMinutes = _startMinutes;
            }
        }

        _timeTypeSelector.SetActive(activate);
        _timeButtonText.gameObject.SetActive(!activate);

        _startTimeText.GetComponent<UIToggle>().isOn = true;

        SetPreviewPeriodText(_startHours == _endHours && _startMinutes == _endMinutes ?
            string.Format("{0:00}:{1:00}", _startHours, _startMinutes) :
                $"{string.Format("{0:00}:{1:00}", _startHours, _startMinutes)} - {string.Format("{0:00}:{1:00}", _endHours, _endMinutes)}");

        _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
        _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);
    }



    public void SetTime(int time, TimeLabelType type)
    {
        _clockSelectable.Select();

        switch (type)
        {
            case TimeLabelType.Hour:
                foreach(TimeLabel label in _hourList)
                {
                    if(label.TimeValue == time)
                        label.SelectTime(true, _startTime, _startTime ? _startTimeColor : _endTimeColor);
                    else
                        if(!label.Selected || (label.Selected && label.IsStartTime == _startTime))
                            label.SelectTime(false);
                }

                if (_startTime)
                {
                    _startHours = time;
                }
                else
                {
                    _endHours = time;
                }  
                break;

            case TimeLabelType.Minute:

                foreach (TimeLabel label in _minuteList)
                {
                    if (label.TimeValue == time)
                        label.SelectTime(true, _startTime, _startTime ? _startTimeColor : _endTimeColor);
                    else
                        if (!label.Selected || (label.Selected && label.IsStartTime == _startTime))
                        label.SelectTime(false);
                }

                if (_startTime)
                    _startMinutes = time;
                else
                    _endMinutes = time;



                if (_startTime)
                    _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
                else
                    _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);



                if (_startTime)
                {
                    _endTimeText.GetComponent<UIToggle>().isOn = true;
                }
                else
                {
                    if (_endHours == _startHours && _endMinutes < _startMinutes)
                    {
                        _correctPeriod = false;
                    }
                    SetActiveTimeSelector(false);      
                }
                break;
        }

        if(_endHours < _startHours)
        {
            _correctPeriod = false;
            _endTimeText.color = _incorrectPeriodColor;



            if (!_startTime)
            {
                foreach (TimeLabel label in _hourList)
                {
                    if (label.TimeValue == _endHours)
                        label.SelectTime(true, _startTime, _incorrectPeriodColor);
                }
                foreach (TimeLabel label in _minuteList)
                {
                    if (label.TimeValue == _endMinutes)
                        label.SelectTime(true, _startTime, _incorrectPeriodColor);
                }
            }
        }

        else
        {
            if(_endHours == _startHours && _endMinutes < _startMinutes)
            {
                _correctPeriod = false;
                _endTimeText.color = _incorrectPeriodColor;



                if (!_startTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endHours)
                            label.SelectTime(true, _startTime, _incorrectPeriodColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endMinutes)
                            label.SelectTime(true, _startTime, _incorrectPeriodColor);
                    }
                }
            }
            else
            {
                _correctPeriod = true;
                _endTimeText.color = _endTimeColor;

                if (!_startTime)
                {
                    foreach (TimeLabel label in _hourList)
                    {
                        if (label.TimeValue == _endHours)
                            label.SelectTime(true, _startTime, _startTime ? _startTimeColor : _endTimeColor);
                    }
                    foreach (TimeLabel label in _minuteList)
                    {
                        if (label.TimeValue == _endMinutes)
                            label.SelectTime(true, _startTime, _startTime ? _startTimeColor : _endTimeColor);
                    }
                }
            }
        }

        if(_startTime)
            _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
        else
            _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);
    }



    public void SelectTimeType(bool startTime)
    {
        _startTime = startTime;

        ClearClockMarks();
        EnableMarks();

        if (_startTime)
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
        return new DateTime(1, 1, 1, _startHours, _startMinutes, 0);
    }



    public DateTime GetEndPeriod()
    {
        return new DateTime(1, 1, 1, _endHours, _endMinutes, 0);
    }



    public void SetPeriod(DateTime start, DateTime end)
    {
        ClearClockMarks();

        _startHours = start.Hour;
        _startMinutes = start.Minute;
        _endHours = end.Hour;
        _endMinutes = end.Minute;

        _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
        _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);

        EnableMarks();
    }



    public void SetPreviewPeriodText(string text)
    {
        _timeButtonText.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_timeButtonText.rectTransform);
    }



    public void EnableMarks()
    {
        if (_startTime)
        {
            foreach (TimeLabel label in _hourList)
            {
                if (label.TimeValue == _startHours)
                    label.SelectTime(true, true, _startTimeColor);

            }

            foreach (TimeLabel label in _minuteList)
            {
                if (label.TimeValue == _startMinutes)
                    label.SelectTime(true, true, _startTimeColor);

            }
        }
        else
        {
            foreach (TimeLabel label in _hourList)
            {

                if (label.TimeValue == _endHours)
                    label.SelectTime(true, false, _endTimeColor);
            }

            foreach (TimeLabel label in _minuteList)
            {

                if (label.TimeValue == _endMinutes)
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
        _startHours = 0;
        _startMinutes = 0;
        _endHours = 0;
        _endMinutes = 0;

        _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
        _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);

        ClearClockMarks();
    }



    public GameObject GetClockGameObject()
    {
        return _clock.gameObject;
    }



    public bool GetStartTime()
    {
        return _startTime;
    }

    #region ClockGenerator
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



            var s = Instantiate(_timeLabel, _clock);

            s.transform.localPosition = new Vector3(x, y, 0);

            s.transform.localScale = s.transform.localScale * 1.5f;

            s.LabelType = TimeLabelType.Hour;
            s.Clock = this;



            if (i == 0)
            {
                s.LabelText.text = "12";
                s.TimeValue = 12;
            }
            else
            {
                s.LabelText.text = i.ToString();
                s.TimeValue = i;
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



            var s = Instantiate(_timeLabel, _clock);

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
    #endregion
}
