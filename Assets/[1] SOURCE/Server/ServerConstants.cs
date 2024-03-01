using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum Role
{
    Common,
    Organization,
    Fund,
    Admin
}

public class ServerConstants : MonoBehaviour
{
    public static ServerConstants Instance;

    public string ServerAddress { get => "http://82.97.243.104/api/"; }

    public string SignIn { get => "signin"; } 
    public string SignUp { get => "signup"; }

    public string GetProfileIcon { get => "upload/profileIcon/"; }  
    public string GetProfile { get => "profile"; }  
    public string UploadUserIcon { get => "upload/profileIcon"; }

    public string GetUsersByEmailPattern { get => "users"; }

    public string GetChats { get => "chats"; }
    public string GetChatMessages { get => "chat/messages"; }


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }



    public async Task<ResponseBody> AuthAsync(string email, string password)
    {
        AuthBody authBody = new AuthBody(email, password);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var s = JsonUtility.ToJson(authBody);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(SignIn, content);
        TokenResponse? tokenBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            tokenBody = JsonUtility.FromJson<TokenResponse>(tokenBodyString);

            AccountManager.Instance.SetToken(tokenBody);

            return new ResponseBody(true, "Вы вошли!");
        }
        else
        {
            var r = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return new ResponseBody(false, response.Content.ToString());
        }
    }



    public async Task<ResponseBody> RegisterAsync(string email, string fullname, string password)
    {
        RegisterBody registerBody = new RegisterBody(email, fullname, password);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var s = JsonUtility.ToJson(registerBody);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(SignUp, content);
        TokenResponse? tokenBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            tokenBody = JsonUtility.FromJson<TokenResponse>(tokenBodyString);

            AccountManager.Instance.SetToken(tokenBody);

            return new ResponseBody(true, "Вы создали аккаунт и вошли!");
        }
        else
        {
            return new ResponseBody(false, response.Content.ToString());
        }
    }



    public async Task<ProfileData> GetProfileAsync()
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var response = await client.GetAsync(GetProfile);
        ProfileData? profileBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var profileBodyString = await response.Content.ReadAsStringAsync();
            profileBody = JsonUtility.FromJson<ProfileData>(profileBodyString);

            print(profileBody.email);

            return profileBody;
        }
        else
        {
            return null;
        }
    }



    public async Task<ResponseBody> CheckTokenIsValid()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("нет интернета");
            return null;
        }

        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        try { client.DefaultRequestHeaders.Add("Authorization", PlayerPrefs.GetString("AccessToken")); }
        catch { print("Токен отсутствует"); }

        var response = await client.GetAsync(GetProfile);
        ProfileData? profileBody = null;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            ResponseBody responseBody = new ResponseBody(true, "Токен валиден");
            AccountManager.Instance.SetToken(new TokenResponse(PlayerPrefs.GetString("AccessToken"), PlayerPrefs.GetString("RefreshToken")));
            return responseBody;
        }
        else
        {
            print(response.StatusCode);
            ResponseBody responseBody = new ResponseBody(false, "Токен истек");
            return responseBody;
        }
    }



    public async Task UploadUserIconAsync(byte[] bytes)
    {
        //StartCoroutine(UploadAvatar(bytes));
        //return;

        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var formData = new MultipartFormDataContent();
        var byteArray = new ByteArrayContent(bytes);
        byteArray.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = "file.txt"
        };
        formData.Add(byteArray);
        byteArray.Headers.Add("Content-Type", "application/octet-stream");
        formData.Add(byteArray, "file");

        var response = await client.PostAsync(UploadUserIcon, formData);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            print("Аватарка загружена");
        }
        else
        {
            print(response.StatusCode);
            print(response.RequestMessage);
        }
    }



    public async Task<Texture2D> GetAvatarAsync()
    {
        Texture2D s = null;

        StartCoroutine(DownloadTexture((tex) =>
        {
            s = tex;
        }));

        return s;
    }



    public IEnumerator DownloadTexture(Action<Texture2D> response)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(AccountManager.Instance.ProfileData.urlIcon);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            response(((DownloadHandlerTexture)request.downloadHandler).texture);
    }



    public IEnumerator UploadAvatar(byte[] b)
    {
        WWWForm form = new WWWForm();
        byte[] bytes = b;

        form.AddBinaryData("avatar", bytes, "avatar.jpg", "image/jpg");

        Dictionary<string, string> headers = new Dictionary<string, string> { { "Authorization", AccountManager.Instance.TokenResponse.accessToken } };
        WWW w = new WWW(ServerAddress + UploadUserIcon, b, headers);

        yield return w;

        if(w.error != null)
        {
            print(w.error);
            print(w.text);
        }
        else
        {
            print("Аватарка загружена");
        }
    }



    public async Task<List<ProfileData>> GetUsersByEmailPatternAsync(string pattern)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var response = await client.GetAsync(GetUsersByEmailPattern + $"?emailPattern={pattern}");
        List<ProfileData>? profileBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var profileBodyString = await response.Content.ReadAsStringAsync();
            print(profileBodyString);
            profileBody = JsonConvert.DeserializeObject<List<ProfileData>>(profileBodyString);

            return profileBody;
        }
        else
        {
            return null;
        }
    }



    public async Task<List<TaskChatBody>> GetChatsAsync()
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var response = await client.GetAsync(GetChats);
        List<TaskChatBody> ? chatsBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var chatsBodyString = await response.Content.ReadAsStringAsync();
            print(chatsBodyString);
            chatsBody = JsonConvert.DeserializeObject<List<TaskChatBody>>(chatsBodyString);

            return chatsBody;
        }
        else
        {
            return null;
        }
    }



    public async Task<List<MessageBody>> GetChatMessagesAsync(string chatId, int count)
    {

        DynamicDataLoadingOptions chatMessagesBody = new DynamicDataLoadingOptions(count, 0);

        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var s = JsonUtility.ToJson(chatMessagesBody);
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(GetChatMessages + $"?chatId={chatId}", content);
        List<MessageBody>? messagesBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var messagesBodyString = await response.Content.ReadAsStringAsync();
            messagesBody = JsonConvert.DeserializeObject<List<MessageBody>>(messagesBodyString);

            return messagesBody;
        }
        else
        {
            print(response.RequestMessage);
            return null;
        }
    }
}

public class ResponseBody
{
    public bool Success;
    public string Message;

    public ResponseBody(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}



public class RegisterBody
{
    public string email;
    public string fullname;
    public string password;

    public RegisterBody(string email, string fullname, string password)
    {
        this.email = email;
        this.fullname = fullname;
        this.password = password;
    }
}



public class AuthBody
{
    public string email;
    public string password;

    public AuthBody(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}



public class DynamicDataLoadingOptions
{
    public int count;
    public int loadPosition;

    public DynamicDataLoadingOptions(int count, int loadPosition)
    {
        this.count = count;
        this.loadPosition = loadPosition;
    }
}



[Serializable]
public class TokenResponse
{
    public string accessToken;
    public string refreshToken;

    public TokenResponse(string accessToken, string refreshToken)
    {
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
    }
}



[Serializable]
public class ProfileData
{
    public string email;
    public Role role;
    public string urlIcon;

    public ProfileData(string email, Role role, string urlIcon)
    {
        this.email = email;
        this.role = role;
        this.urlIcon = urlIcon;
    }
}



[Serializable]
public class TaskChatBody
{
    public string id;
    public string name;
    public string imageUrl;
    public int countOfUnreadMessages;
    public MessageBody lastMessage;
    public ChatUserInfo[] participants;

    public TaskChatBody(string id, string name, string imageUrl, int countOfUnreadMessages, ChatUserInfo[] participants)
    {
        this.id = id;
        this.name = name;
        this.imageUrl = imageUrl;
        this.countOfUnreadMessages = countOfUnreadMessages;
        this.participants = participants;
    }
}



[Serializable]
public class ChatUserInfo
{
    public string id;
    public string email;
    public string imageUrl;

    public ChatUserInfo(string id, string email, string imageUrl)
    {
        this.id = id;
        this.email = email;
        this.imageUrl = imageUrl;
    }
}



[Serializable]
public class MessageBody
{
    public Guid Id;
    public ChatMessageType Type;
    public string Content;
    public DateTime Date = DateTime.UtcNow;
    public Guid SenderId;
}



[Serializable]
public class CreateMessageBody
{
    public ChatMessageType Type { get; set; }

    public string Content { get; set; }

    public override string ToString()
    {
        return $"Content: {Content} sending";
    }
}



public class ChatMessageInfo
{
    public Guid ChatId;
    public ChatType ChatType;
    public MessageBody Message;
}



public enum ChatType
{
    Task,
    Personal
}



public enum ChatMessageType
{
    Text,
    File
}
