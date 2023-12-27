using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    private string _regURI = "https://localhost:7031/api/Auth/register";
    private string _authURI = "https://localhost:7031/api/Auth/login";

    [SerializeField] private TMP_InputField _loginAuth;
    [SerializeField] private TMP_InputField _passAuth;

    [Space]

    [SerializeField] private TMP_InputField _firstName;
    [SerializeField] private TMP_InputField _lastName;
    [SerializeField] private TMP_InputField _userName;
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;

    [SerializeField] private TextMeshProUGUI _firstNameText;
    [SerializeField] private TextMeshProUGUI _lastNameText;

    [SerializeField] private TextMeshProUGUI _resultRegText;
    [SerializeField] private TextMeshProUGUI _resultLoginText;

    public string Token;

    public UnityEvent OnAuth;

    public async void Authorization()
    {
        await AuthAsync();
    }

    public async void Registration()
    {
        await RegisterAsync();
    }

    public async Task AuthAsync()
    {
        AuthModel user = new AuthModel(_loginAuth.text, _passAuth.text);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7031/api/Auth/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var s = JsonUtility.ToJson(user);
        print(s);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("login", content);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            var t = JsonUtility.FromJson<Token>(tokenBodyString);

            print(t.message);

            if (t.isSucceed)
            {
                _resultLoginText.text = "Authorized";
                OnAuth?.Invoke();
            }
            else
            {
                _resultLoginText.text = "Invalid username or password";
            }

            Token = t.message;

            GetUserInfo(Token);
        }
        else
        {
            print(response.Content);
            print(response.RequestMessage);
        }
    }

    public async Task RegisterAsync()
    {
        RegisterModel user = new RegisterModel(_firstName.text, _lastName.text, _userName.text, _email.text, _password.text);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7031/api/Auth/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var s = JsonUtility.ToJson(user);
        print(s);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("register", content);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            var t = JsonUtility.FromJson<Token>(tokenBodyString);

            print(t.message);

            _resultRegText.text = t.message;
        }
        else
        {
            print(response.Content);
            print(response.RequestMessage);
        }
    }

    public Dictionary<string, string> GetUserInfo(string token)
    {
        using (var client = CreateClient(token))
        {
            var response = client.GetAsync("https://localhost:7031/api/Auth/getUserInfo?token=" + Token).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            var u = JsonConvert.DeserializeObject<UserDataModel>(userDictionary.Values.ElementAt(1));

            print(u.FirstName + " " + u.LastName);

            _firstNameText.text = u.FirstName;
            _lastNameText.text = u.LastName;

            return userDictionary;
        }
    }

    public HttpClient CreateClient(string accessToken = "")
    {
        var client = new HttpClient();
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
        return client;
    }
}

[Serializable]
class AuthModel
{
    public string userName;
    public string password;

    public AuthModel(string login, string password)
    {
        this.userName = login;
        this.password = password;
    }
}

[Serializable]
class RegisterModel
{
    public string FirstName;
    public string LastName;
    public string UserName;
    public string Email;
    public string Password;

    public RegisterModel(string firstName, string lastName, string userName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Password = password;
    }
}

[Serializable]
class Token
{
    public bool isSucceed;
    public string message;
}

[Serializable]
class UserDataModel
{
    public string FirstName;
    public string LastName;
}

