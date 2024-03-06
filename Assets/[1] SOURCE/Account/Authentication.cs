using AdvancedInputFieldPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    [Header("Auth")]
    [SerializeField] private AdvancedInputField _signInIdentifierField;
    [SerializeField] private AdvancedInputField _signInNicknameField;
    [SerializeField] private AdvancedInputField _signInPasswordField;

    [Header("Register")]
    [SerializeField] private AdvancedInputField _signUpIdentifierField;
    [SerializeField] private AdvancedInputField _signUpFullnameField;
    [SerializeField] private AdvancedInputField _signUpPasswordField;

    [SerializeField] private TextMeshProUGUI _changeMethodText;

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
        ResponseBody request = await ServerConstants.Instance.AuthAsync(
            _signInIdentifierField.Text,
            _signInPasswordField.Text,
            GetAuthMethod()
            );
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
        ResponseBody request = await ServerConstants.Instance.RegisterAsync(
            _signInIdentifierField.Text,
            _signInNicknameField.Text,
            _signInPasswordField.Text,
            GetAuthMethod()
            );

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

    

    private AuthenticationMethod GetAuthMethod()
    {
        AuthenticationMethod m = AuthenticationMethod.Email;

        if (_signInIdentifierField.Text.Contains("@"))
            m = AuthenticationMethod.Email;
        else
            m = AuthenticationMethod.Phone;

        return m;
    }



    //public void ChangeMethod()
    //{
    //    _isPhoneMethod = !_isPhoneMethod;

    //    _signInIdentifierField.PlaceHolderText = _isPhoneMethod ? "Phone" : "Email";
    //    _signInIdentifierField.KeyboardType = _isPhoneMethod ? KeyboardType.PHONE_PAD : KeyboardType.EMAIL_ADDRESS;
    //    _signUpIdentifierField.PlaceHolderText = _isPhoneMethod ? "Phone" : "Email";
    //    _signUpIdentifierField.KeyboardType = _isPhoneMethod ? KeyboardType.PHONE_PAD : KeyboardType.EMAIL_ADDRESS;

    //    _changeMethodText.text = _isPhoneMethod ? "Enter with email" : "Enter with phone number";
    //}
}

