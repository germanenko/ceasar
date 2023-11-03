using HutongGames.PlayMaker.Ecosystem.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public Image SelectIndicator;

    public bool Selected;

    public bool IsStartTime;

    public void SetTime()
    {
        Clock.SetTime(TimeValue, LabelType);
    }



    public void SelectTime(bool select, bool isStartTime = false, Color indicatorColor = new Color())
    {
        Selected = select;
        IsStartTime = isStartTime;

        if (select)
        {
            SelectIndicator.gameObject.SetActive(true);
            SelectIndicator.color = indicatorColor;
            LabelText.color = Color.white;
        }
        else
        {
            SelectIndicator.gameObject.SetActive(false);
            LabelText.color = Color.black;
        }
    }
}
