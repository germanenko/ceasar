using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    [SerializeField] private int _day;

    [SerializeField] private TextMeshProUGUI _dayText;
    public TextMeshProUGUI DayText => _dayText;

    [SerializeField] private bool _interactable;

    [SerializeField] private Image _image;

    public void SetDay(int day)
    {
        _day = day;
        _dayText.text = day.ToString();
    }

    public void SetInteractable(bool interactable)
    {
        _interactable = interactable;

        if(interactable)
            _image.color = Color.white;
        else
            _image.color = Color.gray;
    }
}
