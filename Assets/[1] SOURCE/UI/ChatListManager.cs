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
using Germanenko.Source;

public class ChatListManager : MonoBehaviour
{

    public List<TaskChatBody> Chats;
    public TaskChatBody OpeningChat;

    [SerializeField] private List<ChatItem> _chatItems;

    [SerializeField] private Transform _chatsParent;

    [SerializeField] private ChatItem _chatButtonPrefab;

    public WebSocket WS = new WebSocket("wss://itsmydomain.ru/chat");
    public WebSocket MainWS = new WebSocket("wss://itsmydomain.ru/main");

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
                    UnityMainThreadDispatcher.Instance().Enqueue(async () => 
                    { 
                        chatItem.UpdateLastMessage(m.Message.Content, m.Message.Date);
                        chatItem.SetLastMessageBold(true);

                        var ms = await ServerConstants.Instance.GetChatMessagesAsync(chatItem.ChatInfo.id, 50);
                        Toolbox.Get<Tables>().ClearMessagesFromChat(chatItem.ChatInfo.id);
                        foreach (var m in ms)
                        {
                            Toolbox.Get<Tables>().SaveMessage(m.Id.ToString(), chatItem.ChatInfo.id, m.Content, m.SenderId.ToString(), m.Date.ToString(), m.Type.ToString());
                        }
                    });
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
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("нет интернета");

            foreach (var chat in Toolbox.Get<Tables>().GetAllChats())
            {
                Chats.Add(new TaskChatBody(chat.Id, chat.Name, chat.Image, 0, new ChatUserInfo[2]));
            }

            GenerateChatList();
            return;
        }

        Chats = await ServerConstants.Instance.GetChatsAsync();

        foreach (var chat in Chats)
        {
            var ms = await ServerConstants.Instance.GetChatMessagesAsync(chat.id, 50);
            Toolbox.Get<Tables>().ClearMessagesFromChat(chat.id);
            foreach (var m in ms)
            {
                Toolbox.Get<Tables>().SaveMessage(m.Id.ToString(), chat.id, m.Content, m.SenderId.ToString(), m.Date.ToString(), m.Type.ToString());
            }
        }

        try
        {
            foreach (var chat in Chats)
            {
                if (!Toolbox.Get<Tables>().HaveChat(chat.id))
                {
                    print("Полученного чата нет - добавляю");
                    Toolbox.Get<Tables>().SaveChat(chat.id, chat.name, "Personal", chat.countOfUnreadMessages, chat.imageUrl);
                }

                if(chat.lastMessage != null)
                {
                    var ms =  await ServerConstants.Instance.GetChatMessagesAsync(chat.id, 50);
                }

                if(chat.countOfUnreadMessages > 0)
                {
                    Toolbox.Get<Tables>().UpdateUnreadMessagesCount(chat.id, chat.countOfUnreadMessages);
                }
            }
        }
        catch (Exception ex) { print(ex); }


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
            ci.SetUnreadMessages(Toolbox.Get<Tables>().GetUnreadMessagesCount(chat.id));

            _chatItems.Add(ci);
        }

        MainHeaders["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;

        MainWS.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        MainWS.CustomHeaders = MainHeaders;

        MainWS.ConnectAsync();

        MainWS.OnOpen += MainWS_OnOpen;
        MainWS.OnMessage += MainWS_OnMessage;
        MainWS.OnClose += MainWS_OnClose;
    }

    private void MainWS_OnClose(object sender, CloseEventArgs e)
    {
        print($"main {e.Reason}");
    }

    public void OpenChat(TaskChatBody chat)
    {
        if (chat.id == "")
        {
            _chatManager.OpenChat(chat);
            return;
        }
       
        Headers["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;
        Headers["chatId"] = chat.id;
       
        OpeningChat = chat;
       
        WS.CustomHeaders = Headers;

        WS.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        WS.ConnectAsync();
       
        WS.OnOpen += WS_OnOpen;
        WS.OnClose += WS_OnClose;
    }

    private void WS_OnClose(object sender, CloseEventArgs e)
    {
        print(e.Reason + " " + e.Code);
    }

    public async void FindUsersByIdentifierPattern(string pattern)
    {
        if(pattern == "")
        {
            GenerateChatList();
            return;
        }

        _chatsParent.DestroyChildren();
        _chatItems.Clear();

        List<ProfileData> users = await ServerConstants.Instance.GetUsersByIdentifierPatternAsync(pattern);

        foreach (ProfileData user in users)
        {
            var c = Pooler.Instance.Spawn(PoolType.Entities, _chatButtonPrefab.gameObject, default, default, _chatsParent);

            ChatItem ci = c.GetComponent<ChatItem>();

            TaskChatBody chat = new TaskChatBody("", user.nickname, user.urlIcon, 0, new ChatUserInfo[2] {new ChatUserInfo("", AccountManager.Instance.ProfileData.identifier, AccountManager.Instance.ProfileData.urlIcon, AccountManager.Instance.ProfileData.userTag, AccountManager.Instance.ProfileData.identifierType), new ChatUserInfo("", user.identifier, user.urlIcon, user.userTag, user.identifierType) });

            ci.Init(chat, this);
            ci.SetUnreadMessages(0);

            _chatItems.Add(ci);
        }
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



    private void OnApplicationQuit()
    {
        MainWS.Close();
    }
}
