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
    }



    private void MainWS_OnMessage(object sender, MessageEventArgs e)
    {
        print("����� ���������!");
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
        print("��������� � �������� ������");
    }



    public async void GetChats()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            print("��� ���������");

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
            //if (chat.lastMessage != null)
            //{
                var ms = await ServerConstants.Instance.GetChatMessagesAsync(chat.id, 50);
                Toolbox.Get<Tables>().ClearMessagesFromChat(chat.id);
                foreach (var m in ms)
                {
                    Toolbox.Get<Tables>().SaveMessage(m.Id.ToString(), chat.id, m.Content, m.SenderId.ToString(), m.Date.ToString(), m.Type.ToString());
                }
            //}
        }

        try
        {
            //if (Toolbox.Get<Tables>().GetAllChats().Count < Chats.Count)
            //{
                foreach (var chat in Chats)
                {
                    if (!Toolbox.Get<Tables>().HaveChat(chat.id))
                    {
                        print("����������� ���� ��� - ��������");
                        Toolbox.Get<Tables>().SaveChat(chat.id, chat.name, "Personal", chat.countOfUnreadMessages, chat.imageUrl);
                    }

                    if(chat.lastMessage != null)
                    {
                        var ms =  await ServerConstants.Instance.GetChatMessagesAsync(chat.id, 50);

                        foreach (var item in ms)
                        {
                            print(item.Content);
                        }
                    }

                    if(chat.countOfUnreadMessages > 0)
                    {
                        Toolbox.Get<Tables>().UpdateUnreadMessagesCount(chat.id, chat.countOfUnreadMessages);
                    }
                }
            //}
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

        MainWS.CustomHeaders = MainHeaders;

        MainWS.ConnectAsync();

        MainWS.OnOpen += MainWS_OnOpen;
        MainWS.OnMessage += MainWS_OnMessage;
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



    public async void FindUsersByEmailPattern(string pattern)
    {
        if(pattern == "")
        {
            GenerateChatList();
            return;
        }

        _chatsParent.DestroyChildren();
        _chatItems.Clear();

        List<ProfileData> users = await ServerConstants.Instance.GetUsersByEmailPatternAsync(pattern);

        foreach (ProfileData user in users)
        {
            var c = Pooler.Instance.Spawn(PoolType.Entities, _chatButtonPrefab.gameObject, default, default, _chatsParent);

            ChatItem ci = c.GetComponent<ChatItem>();

            TaskChatBody chat = new TaskChatBody("", user.nickname, user.urlIcon, 0, new ChatUserInfo[2]);

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
