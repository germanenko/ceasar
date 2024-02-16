using AdvancedInputFieldPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    [Header("Auth")]
    [SerializeField] private AdvancedInputField _signInEmailField;
    [SerializeField] private AdvancedInputField _signInPasswordField;

    [Header("Register")]
    [SerializeField] private AdvancedInputField _signUpEmailField;
    [SerializeField] private AdvancedInputField _signUpFullnameField;
    [SerializeField] private AdvancedInputField _signUpPasswordField;

    private async void Awake()
    {
        var tokenIsValid = await ServerConstants.Instance.CheckTokenIsValid();

        if (tokenIsValid.Success)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public async void AuthAsync()
    {
        ResponseBody request = await ServerConstants.Instance.AuthAsync(_signInEmailField.Text, _signInPasswordField.Text);
        if (request.Success)
        {
            print(request.Message);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            print(request.Message);
        }
    }

    public async void RegisterAsync()
    {
        ResponseBody request = await ServerConstants.Instance.RegisterAsync(_signUpEmailField.Text, _signUpFullnameField.Text, _signUpPasswordField.Text);
        if (request.Success)
        {
            print(request.Message);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            print(request.Message);
        }
    }

}

