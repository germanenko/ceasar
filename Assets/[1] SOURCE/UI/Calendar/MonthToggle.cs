using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonthToggle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _monthNameText;

    [SerializeField] private DateTime _month;

    [SerializeField] private Calendar _calendar;

    public UIToggle UIToggle;

    public void Init(DateTime month, string monthName, Calendar calendar)
    {
        _month = month;
        _calendar = calendar;
        _monthNameText.text = monthName;
    }



    public void SetBoldOnMonthName()
    {
        _monthNameText.fontStyle = FontStyles.Bold;
    }



    public void SelectMonth()
    {
        _calendar.SetMonth(_month);
    }
}
