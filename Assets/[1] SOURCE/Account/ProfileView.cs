using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _emailText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private Image _avatar;

    private void Start()
    {
        LoadInfo();
    }

    public void LoadInfo()
    {
        _emailText.text = AccountManager.Instance.ProfileData.email;
        _roleText.text = AccountManager.Instance.ProfileData.role.ToString();
    }
}
