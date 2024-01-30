using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTimeItem : MonoBehaviour
{
    [SerializeField] private Calendar _calendar;
    [SerializeField] private Clock _clock;

    

    public void CheckDateIsPeriod(bool isPeriod)
    {
        print(isPeriod);
        _clock.gameObject.SetActive(!isPeriod);
    }
}