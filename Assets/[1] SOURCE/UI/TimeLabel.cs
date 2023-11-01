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

    public void SetTime()
    {
        Clock.SetTime(TimeValue, LabelType);
    }
}
