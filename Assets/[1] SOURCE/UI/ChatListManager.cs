using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using Doozy.Runtime.Common.Extensions;
using Germanenko.Framework;
using System.Threading;
using Doozy.Runtime.UIManager.Containers;
using WebSocketSharp;
using PimDeWitte.UnityMainThreadDispatcher;
using Newtonsoft.Json;

public class ChatListManager : MonoBehaviour
{

    public TaskChatBody[] Chats;
    public TaskChatBody OpeningChat;

    [SerializeField] private List<ChatItem> _chatItems;

    [SerializeField] private Transform _chatsParent;

    [SerializeField] private ChatItem _chatButtonPrefab;

    public WebSocket WS = new WebSocket("ws://82.97.243.104/chat");
    public WebSocket MainWS = new WebSocket("ws://82.97.243.104/main");

    [SerializeField] private ChatManager _chatManager;

    Dictionary<string, string> MainHeaders = new Dictionary<string, string>
    {
        { "Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIwYWU2NDM0My1jY2MzLTQzNTQtYmU2Ni03YmUyODk3ZGI3NWEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb21tb24iLCJleHAiOjE3MDc3MTYyMTh9.6li5bUA62z2aJ3OAu8mL_CqehMPjnw4oDG5k2hzXLuA" }
    };

    Dictionary<string, string> Headers = new Dictionary<string, string>
    {
        { "Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIwYWU2NDM0My1jY2MzLTQzNTQtYmU2Ni03YmUyODk3ZGI3NWEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb21tb24iLCJleHAiOjE3MDc3MTYyMTh9.6li5bUA62z2aJ3OAu8mL_CqehMPjnw4oDG5k2hzXLuA" },
        { "chatId", "653BCFFA-6D86-4370-BF4B-7C02FD90B54E" }
    };



    private void Start()
    {
        GetChats();

        MainHeaders["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;

        MainWS.CustomHeaders = MainHeaders;

        MainWS.ConnectAsync();

        MainWS.OnOpen += MainWS_OnOpen;
        MainWS.OnMessage += MainWS_OnMessage;
    }

    private void MainWS_OnMessage(object sender, MessageEventArgs e)
    {
        print("Новое сообщение!");
        try
        {
            var m = JsonConvert.DeserializeObject<ChatMessageInfo>(e.Data);

            var chatId = m.ChatId;

            foreach (var chatItem in _chatItems)
            {
                if (chatItem.ChatInfo.id == chatId.ToString())
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => chatItem.UpdateLastMessage(m.Message.Content, m.Message.Date));
                }
            }
        }
        catch(Exception ex)
        {
            print(ex);
        }
    }

    private void MainWS_OnOpen(object sender, EventArgs e)
    {
        print("Подключен к главному сокету");
    }

    public async void GetChats()
    {
        Chats = await ServerConstants.Instance.GetChatsAsync();

        GenerateChatList();
    }



    public void GenerateChatList()
    {
        _chatsParent.DestroyChildren();

        foreach (var chat in Chats)
        {
            var c = Pooler.Instance.Spawn(PoolType.Entities, _chatButtonPrefab.gameObject, default, default, _chatsParent);

            ChatItem ci = c.GetComponent<ChatItem>();

            ci.Init(chat, this);

            _chatItems.Add(ci);
        }
    }



    public void OpenChat(TaskChatBody chat)
    {
        Headers["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;
        Headers["chatId"] = chat.id;

        OpeningChat = chat;

        WS.CustomHeaders = Headers;

        WS.ConnectAsync();

        WS.OnOpen += WS_OnOpen;

        //foreach (var header in Headers)
        //{
        //    WS.Options.SetRequestHeader(header.Key, header.Value);
        //}

        //print(WS.Options);

        //await WS.ConnectAsync(new Uri("ws://82.97.249.229/taskChat"), CancellationToken.None);

        //if(WS.State == WebSocketState.Open)
        //{
        //    _chatManager.OpenChat(chat);
        //}

    }

    private void WS_OnMessage(object sender, MessageEventArgs e)
    {
        print(e.Data);
    }

    private void WS_OnOpen(object sender, EventArgs e)
    {
        print("open");
        UnityMainThreadDispatcher.Instance().Enqueue(() => _chatManager.OpenChat(OpeningChat, WS));
        WS.OnOpen -= WS_OnOpen;
    }
}
