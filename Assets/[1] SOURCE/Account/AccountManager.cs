using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public ProfileData ProfileData;

    public static AccountManager Instance;

    public TokenResponse TokenResponse;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetToken(TokenResponse tokenResponse)
    {
        TokenResponse = tokenResponse;

        PlayerPrefs.SetString("AccessToken", tokenResponse.accessToken);
        PlayerPrefs.SetString("RefreshToken", tokenResponse.refreshToken);

        SetProfileData();
    }

    public async void SetProfileData()
    {
        ProfileData = await ServerConstants.Instance.GetProfileAsync();
    }
}
