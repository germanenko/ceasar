using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    [SerializeField] private Calendar _calendar;

    [SerializeField] private int _day;

    [SerializeField] private TextMeshProUGUI _dayText;
    public TextMeshProUGUI DayText => _dayText;

    [SerializeField] private Image _image;

    private void Start()
    {
        print(_day);
    }

    public void SetDay(int day)
    {
        _day = day;
        _dayText.text = day.ToString();
    }



    public void SetCalendar(Calendar calendar)
    {
        _calendar = calendar;
    }



    public void SelectDay()
    {
        print(_day);
        _calendar.SetDay(_day);
    }



    public void SetColor(Color color)
    {
        _image.color = color;
    }



    public void SetInteractable(bool interactable)
    {
        if(interactable)
            _image.color = Color.white;
        else
            _image.color = Color.gray;
    }
}
