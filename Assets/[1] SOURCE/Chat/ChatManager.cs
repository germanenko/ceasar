using Germanenko.Framework;
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
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _messageInput;

    [SerializeField] private GameObject _messageItem;

    [SerializeField] private Transform _messageParent;

    [SerializeField] private List<MessageBody> _messageBodies = new List<MessageBody>();

    [SerializeField] private Dictionary<string, string> _messages;

    private void Start()
    {
        LoadMessages();
    }



    public void LoadMessages()
    {
        _messageBodies.Clear();

        int messageCount = _messageParent.childCount;

        for (int i = 0; i < messageCount; i++)
        {
            print("del " + i);
            Pooler.Instance.Despawn(PoolType.Entities, _messageParent.GetChild(0).gameObject);
        }

        _messageBodies = LoadMessagesAsync();

        foreach (var item in _messageBodies)
        {
            var m = Pooler.Instance.Spawn(PoolType.Entities, _messageItem, default, default, _messageParent);
            m.GetComponent<MessageItem>().Init(item.Sender, item.Message);
            print($"{item.Sender}: {item.Message}");
        }
    }



    public async void SendMessage()
    {
        if (_messageInput.text == "") return;

        await SendMessageAsync();
    }



    private async Task SendMessageAsync()
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7031/api/Chat/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        MessageBody m = new MessageBody() { Sender = Account.Instance.FirstName, Message = _messageInput.text };

        string s = JsonUtility.ToJson(m);

        var content = new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = client.PostAsync("send-message", content).Result;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            LoadMessages();
        }
        else
        {
            print(response.Content);
            print(response.RequestMessage);
        }
    }



    public List<MessageBody> LoadMessagesAsync()
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7031/api/Auth/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var response = client.GetAsync("https://localhost:7031/api/Chat/get-messages").Result;

        var result = response.Content.ReadAsStringAsync().Result;

        print(result);

        Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

        print(userDictionary.Values.ElementAt(1));

        var mgs = JsonConvert.DeserializeObject<List<MessageBody>>(userDictionary.Values.ElementAt(1));

        return mgs;
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

public class Response
{
    public bool IsSucceed;
    public string Message;
}

[Serializable]
public class MessageBody
{
    public string Sender;
    public string Message;
}
