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
using Microsoft.AspNetCore.SignalR.Client;
using PimDeWitte.UnityMainThreadDispatcher;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _messageInput;

    [SerializeField] private GameObject _messageItem;

    [SerializeField] private Transform _messageParent;

    [SerializeField] private List<MessageBody> _messageBodies = new List<MessageBody>();

    [SerializeField] private Dictionary<string, string> _messages;

    private HubConnection _hubConnection;

    private void Start()
    {
        Application.targetFrameRate = 60;

        //LoadMessages();

        _hubConnection = new HubConnectionBuilder()
        .WithUrl("https://2262-79-126-114-167.ngrok-free.app/chat-hub")
        .Build();

        _hubConnection.On<string, string>("Receive", (user, message) =>
        {
            try
            {
                print($"{user}: {message}");
                UnityMainThreadDispatcher.Instance().Enqueue(() => { AddMessage(user, message); });
            }
            catch (Exception ex)
            {
                print(ex.Message);
            }
        });

        ConnectToChat();
    }

    private async void ConnectToChat()
    {
        try
        {
            // подключемся к хабу
            await _hubConnection.StartAsync();
            await _hubConnection.InvokeAsync("Send", "Чат", $"{_hubConnection.ConnectionId} вошли в чат");
            //AddMessage("Чат", $"{_hubConnection.ConnectionId} вошли в чат");
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public async void Send()
    {
        try
        {
            await _hubConnection.InvokeAsync("Send", Account.Instance.FirstName, _messageInput.text);
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    private void AddMessage(string user, string message)
    {
        //var m = Pooler.Instance.Spawn(PoolType.Entities, _messageItem, default, default, _messageParent);
        //var m = Instantiate(_messageItem, _messageParent);
        //m.GetComponent<MessageItem>().Init(user, message);
        var m = Instantiate(_messageItem, _messageParent);
        m.GetComponent<MessageItem>().Init(user, message);
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
            BaseAddress = new Uri("https://2262-79-126-114-167.ngrok-free.app/api/Chat/"),
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
            BaseAddress = new Uri("https://2262-79-126-114-167.ngrok-free.app/api/Auth/"),
        };
        client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "69420");

        var response = client.GetAsync("https://2262-79-126-114-167.ngrok-free.app/api/Chat/get-messages").Result;

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
