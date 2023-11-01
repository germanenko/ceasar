using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TimeLabel _timeLabel;

    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private float _hours;
    [SerializeField] private float _minutes;

    void Start()
    {
        //DrawHour(12, 100);
        //DrawMinute(60, 185);
    }



    public void SetTime(float time, TimeLabelType type)
    {
        switch(type)
        {
            case TimeLabelType.Hour:
                _hours = time; 
                break;
            case TimeLabelType.Minute:
                _minutes = time;
                break;
        }

        _timeText.text = string.Format("{0:00}:{1:00}", _hours, _minutes);
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

            var s = Instantiate(_timeLabel, transform);

            s.transform.localPosition = new Vector3(x, y, 0);

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

            s.LabelText.fontSize = 14;

            s.LabelText.text = i.ToString();
            s.TimeValue = i;
            s.Clock = this;
        }
    }
}
