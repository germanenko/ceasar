using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    private string _regURI = "https://6311-92-62-150-155.ngrok-free.app/api/Account/signup";
    private string _authURI = "https://6311-92-62-150-155.ngrok-free.app/api/Account/signin";

    [SerializeField] private TMP_InputField _loginAuth;
    [SerializeField] private TMP_InputField _passAuth;

    [Space]

    [SerializeField] private TMP_InputField _loginReg;
    [SerializeField] private TMP_InputField _passReg;
    [SerializeField] private TMP_InputField _confirmReg;
    [SerializeField] private TMP_InputField _firstNameReg;
    [SerializeField] private TMP_InputField _lastNameReg;

    public async void Authorization()
    {
        //var a = new AuthModel(_loginAuth.text, _passAuth.text);
        //print(JsonUtility.ToJson(a));
        //StartCoroutine(AuthorizationCoroutine(a));

        await AuthAsync();
    }

    public void Registration()
    {

    }

    IEnumerator AuthorizationCoroutine(AuthModel authModel)
    {
        using (UnityWebRequest wr = UnityWebRequest.Post(_authURI, JsonUtility.ToJson(authModel)))
        {
            wr.SetRequestHeader("Content-Type", "application/json");

            yield return wr.SendWebRequest();

            switch (wr.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    print(wr.error + "   " + wr.result);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    print(wr.error + "   " + wr.result);
                    break;
                case UnityWebRequest.Result.Success:
                    print(wr.downloadHandler.text);
                    break;
            }

        }

        yield return null;
    }

    IEnumerator RegistrationCoroutine()
    {
        yield return null;
    }

    public async Task AuthAsync()
    {
        AuthModel student = new AuthModel(_loginAuth.text, _passAuth.text);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://6311-92-62-150-155.ngrok-free.app/api/Account/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var s = JsonUtility.ToJson(student);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("signin", content);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            var t = JsonUtility.FromJson<Token>(tokenBodyString);
            var user = GetUserInfo(t.access_token);

            print(user);
        }
        else
        {
            print(response.Content);
            print(response.StatusCode);
        }
    }



    public HttpClient CreateClient(string accessToken = "")
    {
        var client = new HttpClient();
        if (accessToken != "")
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        }
        return client;
    }



    public Dictionary<string, string> GetUserInfo(string token)
    {
        print(CreateClient(token).BaseAddress);

        using (var client = CreateClient(token))
        {
            var response = client.GetAsync("https://6311-92-62-150-155.ngrok-free.app/api/Account/getProfileData").Result;

            print(response);

            var result = response.Content.ReadAsStringAsync().Result;

            Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            return userDictionary;
        }
    }
}

[Serializable]
class AuthModel
{
    public string login;
    public string password;

    public AuthModel(string login, string password)
    {
        this.login = login;
        this.password = password;
    }
}

[Serializable]
class Token
{
    public string access_token;
    public string username;

    public Token(string access, string username)
    {
        this.access_token = access;
        this.username = username;
    }
}

