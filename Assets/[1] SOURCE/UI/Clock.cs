using Doozy.Runtime.Reactor.Animators;
using FlyingWormConsole3.LiteNetLib.Utils;
using HutongGames.PlayMaker.Ecosystem.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TimeLabel _timeLabel;

    [SerializeField] private bool _startTime;

    [SerializeField] private TextMeshProUGUI _startTimeText;
    [SerializeField] private TextMeshProUGUI _endTimeText;

    [SerializeField] private float _startHours;
    [SerializeField] private float _startMinutes;

    [SerializeField] private float _endHours;
    [SerializeField] private float _endMinutes;

    [SerializeField] List<TimeLabel> _hourList;
    [SerializeField] List<TimeLabel> _minuteList;

    [SerializeField] private Color _startTimeColor;
    [SerializeField] private Color _endTimeColor;



    void Start()
    {
        //DrawHour(12, 120);
        //DrawMinute(60, 230);
    }



    public void SetTime(float time, TimeLabelType type)
    {
        switch(type)
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
                
                if(_startTime)
                    _startHours = time;
                else
                    _endHours = time;
                break;

            case TimeLabelType.Minute:
                if (_startTime)
                    _startMinutes = time;
                else
                    _endMinutes = time;
                break;
        }


        if(_startTime)
            _startTimeText.text = string.Format("{0:00}:{1:00}", _startHours, _startMinutes);
        else
            _endTimeText.text = string.Format("{0:00}:{1:00}", _endHours, _endMinutes);
    }



    public void SelectTime(bool startTime)
    {
        _startTime = startTime;
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



            var s = Instantiate(_timeLabel, transform);

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



            var s = Instantiate(_timeLabel, transform);

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
