using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushManager : MonoBehaviour
{
    [SerializeField] private AndroidNotifications _androidNotifications;

    private void Start()
    {
        try
        {
            _androidNotifications.RequestAuthorization();
            _androidNotifications.RegisterNotificationChannel();

            _androidNotifications.SendNotification("Тест", "Первое уведомление");
        }
        catch (Exception ex)
        {
            print(ex);
        }
    }
}
