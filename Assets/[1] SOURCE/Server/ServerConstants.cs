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

    public string ServerAddress { get => "https://itsmydomain.ru/api/"; }

    public string SignIn { get => "signin"; } 
    public string SignUp { get => "signup"; }

    public string GetProfileIcon { get => "upload/profileIcon/"; }  
    public string GetProfile { get => "profile"; }  
    public string UploadUserIcon { get => "upload/profileIcon"; }

    public string GetUsersByIdentifierPattern { get => "users/identifier"; }

    public string GetChats { get => "chats"; }
    public string GetChatMessages { get => "chat/messages"; }
    public string CreatePersonalChat { get => "chat"; }

    public string CreateBoard { get => "board"; }
    public string GetAllBoards { get => "boards"; }


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }



    public async Task<ResponseBody> AuthAsync(string identifier, string password, AuthenticationMethod method)
    {
        AuthBody authBody = new AuthBody(identifier, password, method);

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
            print(response.StatusCode);
            return new ResponseBody(false, response.Content.ToString());
        }
    }



    public async Task<ResponseBody> RegisterAsync(string identifier, string nickname, string password, AuthenticationMethod method)
    {
        RegisterBody registerBody = new RegisterBody(identifier, nickname, password, method);

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
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            return new ResponseBody(false, tokenBodyString);
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



    public async Task<List<ProfileData>> GetUsersByIdentifierPatternAsync(string pattern)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var response = await client.GetAsync(GetUsersByIdentifierPattern + $"?identifierPattern={pattern}");
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
            var tokenBodyString = await response.Content.ReadAsStringAsync();
            print(tokenBodyString);
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



    public async Task<string> CreatePersonalChatAsync(string identifier)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);
        client.DefaultRequestHeaders.Add("identifier", identifier);

        var s = $"{{ \"name\": \"{identifier}\"}}";
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(CreatePersonalChat, content);
        List<MessageBody>? messagesBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var messagesBodyString = await response.Content.ReadAsStringAsync();
            print("Чат создан");
            return messagesBodyString;
        }
        else
        {
            print(response.RequestMessage);
            return null;
        }
    }



    public async Task CreateBoardAsync(string name)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var s = $"{{ \"name\": \"{name}\"}}";
        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(CreateBoard, content);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            print("Доска создана");
        }
        else
        {
            print($"Доска не создана - {response.Content}");
        }
    }



    public async Task<List<BoardBody>> GetAllBoardsAsync()
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri(ServerAddress),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");
        client.DefaultRequestHeaders.Add("Authorization", AccountManager.Instance.TokenResponse.accessToken);

        var response = await client.GetAsync(GetAllBoards);
        List<BoardBody>? boardsBody = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var boardsBodyString = await response.Content.ReadAsStringAsync();

            boardsBody = JsonConvert.DeserializeObject<List<BoardBody>>(boardsBodyString);

            return boardsBody;
        }
        else
        {
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
    public string identifier;
    public string nickname;
    public string password;
    public AuthenticationMethod method;

    public RegisterBody(string identifier, string nickname, string password, AuthenticationMethod method)
    {
        this.identifier = identifier;
        this.nickname = nickname;
        this.password = password;
        this.method = method;
    }
}



public class AuthBody
{
    public string identifier;
    public string password;
    public AuthenticationMethod method;

    public AuthBody(string identifier, string password, AuthenticationMethod method)
    {
        this.identifier = identifier;
        this.password = password;
        this.method = method;
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
    public string identifier;
    public string nickname;
    public Role role;
    public string urlIcon;
    public string userTag;
    public AuthenticationMethod identifierType;

    public ProfileData(string identifier, string nickname, Role role, string urlIcon, string userTag, AuthenticationMethod identifierType)
    {
        this.identifier = identifier;
        this.nickname = nickname;
        this.role = role;
        this.urlIcon = urlIcon;
        this.userTag = userTag;
        this.identifierType = identifierType;
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
    public string identifier;
    public string imageUrl;
    public string userTag;
    public AuthenticationMethod identifierType;

    public ChatUserInfo(string id, string identifier, string imageUrl, string userTag, AuthenticationMethod identifierType)
    {
        this.id = id;
        this.identifier = identifier;
        this.imageUrl = imageUrl;
        this.userTag = userTag;
        this.identifierType = identifierType;
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



[Serializable]
public class BoardBody
{
    public string id;
    public string name;
}



public enum AuthenticationMethod
{
    Email,
    Phone
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
