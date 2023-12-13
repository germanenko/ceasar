using Germanenko.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    [SerializeField] private int _day;
    [SerializeField] private Dictionary<int, string> _month = new Dictionary<int, string> { 
    { 1, "January" },
    { 2, "February" },
    { 3, "March" },
    { 4, "April" },
    { 5, "May" },
    { 6, "June" },
    { 7, "Jule" },
    { 8, "August" },
    { 9, "September" },
    { 10, "October" },
    { 11, "November" },
    { 12, "December" }};

    [SerializeField] private int _year;

    [SerializeField] private DayButton _dayButtonPrefab;

    [SerializeField] private Transform _dayButtonsParent;

    [SerializeField] private List<DayButton> _dayButtons;

    private void Start()
    {
        GenerateCalendar(2023, 12);
    }



    private void GenerateCalendar(int year, int month)
    {
        foreach (var item in _dayButtons)
        {
            Pooler.Instance.Despawn(PoolType.Entities, item.gameObject);
        }

        _dayButtons.Clear();



        for (int i = 0; i < GetMonthStartDay(year, month); i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(DateTime.DaysInMonth(year, month - 1) - GetMonthStartDay(year, month) + i + 1);
            dayButton.SetInteractable(false);
            _dayButtons.Add(dayButton);
        }



        for (int i = 0; i < DateTime.DaysInMonth(year, month); i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(i + 1);
            dayButton.SetInteractable(true);
            _dayButtons.Add(dayButton);
        }

        int lastDays = 42 - _dayButtons.Count;

        for (int i = 0; i < lastDays; i++)
        {
            DayButton dayButton = SpawnDayButton();

            dayButton.SetDay(i + 1);
            dayButton.SetInteractable(false);
            _dayButtons.Add(dayButton);
        }
    }



    private DayButton SpawnDayButton()
    {
        var d = Pooler.Instance.Spawn(PoolType.Entities, _dayButtonPrefab.gameObject, default, default, _dayButtonsParent);
        DayButton dayButton = d.GetComponent<DayButton>();
        return dayButton;
    }



    private int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        return (int)temp.DayOfWeek - 1;
    }
}
