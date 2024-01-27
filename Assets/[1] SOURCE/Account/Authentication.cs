using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    [Header("Auth")]
    [SerializeField] private TMP_InputField _signInEmailField;
    [SerializeField] private TMP_InputField _signInPasswordField;

    [Header("Register")]
    [SerializeField] private TMP_InputField _signUpEmailField;
    [SerializeField] private TMP_InputField _signUpFullnameField;
    [SerializeField] private TMP_InputField _signUpPasswordField;

    private async void Start()
    {
        var tokenIsValid = await ServerConstants.Instance.CheckTokenIsValid();

        if (tokenIsValid.Success)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public async void AuthAsync()
    {
        ResponseBody request = await ServerConstants.Instance.AuthAsync(_signInEmailField.text, _signInPasswordField.text);
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
        ResponseBody request = await ServerConstants.Instance.RegisterAsync(_signUpEmailField.text, _signUpFullnameField.text, _signUpPasswordField.text);
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

