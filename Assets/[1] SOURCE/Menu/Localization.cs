using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalizationLanguage
{
    Russia = 1,
    USA = 2
}

public class Localization : MonoBehaviour
{
    [SerializeField] private LocalizationLanguage _language;
    public LocalizationLanguage Language => _language;

    public static Localization Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLocalizationLanguage(int language)
    {
        _language = (LocalizationLanguage)language;
    }
}
