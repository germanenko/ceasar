using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum TimeLabelType
{
    Hour,
    Minute
}

public class TimeLabel : MonoBehaviour
{
    public TimeLabelType LabelType;

    public TextMeshProUGUI LabelText;

    public float TimeValue;

    public Clock Clock;

    public GameObject SelectIndicator;

    public void SetTime()
    {
        Clock.SetTime(TimeValue, LabelType);
    }



    public void SelectTime(bool select)
    {
        if (select)
        {
            SelectIndicator.SetActive(true);
            LabelText.color = Color.white;
        }
        else
        {
            SelectIndicator.SetActive(false);
            LabelText.color = Color.black;
        }
    }
}
