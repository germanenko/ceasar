using AdvancedInputFieldPlugin;
using AdvancedInputFieldSamples;
using Doozy.Runtime.UIManager.Containers;
using Germanenko.Framework;
using Germanenko.Source;
using Newtonsoft.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TaskChatBody _chatInfo;

    [SerializeField] private WebSocket WS = new WebSocket("wss://itsmydomain.ru/chat");

    [SerializeField] private AdvancedInputField _messageField;

    [SerializeField] private ChatListManager _chatListManager;

    [SerializeField] private UIView _view;

    [SerializeField] private ChatViewAdapter _chatView;

    [SerializeField] private TextMeshProUGUI _chatNameText;

    [SerializeField] private List<ChatMessages> _messagesFromDB;

    [SerializeField] private GameObject _proposal;

    [SerializeField] private RawImage _chatIcon;

    public void OpenChat(TaskChatBody chatInfo, bool emptyChat)
    {
        _chatInfo = chatInfo;

        if(!emptyChat)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;
            headers["chatId"] = chatInfo.id;

            WS.CustomHeaders = headers;

            WS.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            WS.ConnectAsync();

            WS.OnOpen += WS_OnOpen;
            WS.OnMessage += WS_OnMessage;
            WS.OnClose += WS_OnClose; 
        }
        else
        {
            _view.Show();

            _chatNameText.text = _chatInfo.name;

            GenerateMessageList();
        }
    }

    private void WS_OnClose(object sender, CloseEventArgs e)
    {
        WS.OnOpen -= WS_OnOpen;
        WS.OnMessage -= WS_OnMessage;
        WS.OnClose -= WS_OnClose;
    }

    private void WS_OnOpen(object sender, EventArgs e)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _view.Show();

            _chatNameText.text = _chatInfo.name;

            ServerConstants.Instance.StartCoroutine(ServerConstants.Instance.DownloadChatIconTexture((tex) =>
            {
                _chatIcon.texture = tex;
                _chatIcon.color = Color.white;
            },
            _chatInfo.imageUrl));

            GenerateMessageList();
        });        
    }



    public void CloseChat()
    { 
        _view.Hide();

        //_chatView.ClearAllMessages();

        WS.Close();
    }



    private void WS_OnMessage(object sender, MessageEventArgs e)
    {
        var msg = JsonConvert.DeserializeObject<MessageBody>(e.Data);
        string senderMail = "";
        bool mine = false;
        foreach (var p in _chatInfo.participants)
        {
            if(p.id == msg.SenderId.ToString())
            {
                senderMail = p.nickname;
            }            
        }

        //UnityMainThreadDispatcher.Instance().Enqueue(() => _chatView.AddMessageLeft(msg.Content, senderMail));
        UnityMainThreadDispatcher.Instance().Enqueue(() => { 
        _chatView.Data.InsertOneAtEnd(new ChatMessageItemModel()
        {
            Message = msg,
            IsMine = mine
        });

        _chatView.SetNormalizedPosition(0);
        });


        

        print($"Получено сообщение от {senderMail}: {msg.Content}");
    }

    public async void SendMessage()
    {
        if (_messageField.Text.Length == 0) return;

        var messageBody = new CreateMessageBody
        {
            Type = ChatMessageType.Text,
            Content = _messageField.Text,
        };

        var message = new SentMessage
        {
            MessageBody = messageBody,
            LastMessageReadId = null
        };

        //var str = SerializeObject(message);
        //WS.Send(str);
        if(WS.ReadyState == WebSocketState.Open)
        {
            SendMessageAsync(message);
        }
        else
        {
            try
            {
                string chatId = await ServerConstants.Instance.CreatePersonalChatAsync(_chatInfo.participants[1].identifier);

                chatId = chatId.Replace("\"", "");
                print(chatId);
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Authorization", "" },
                    { "chatId", "" }
                };

                headers["Authorization"] = AccountManager.Instance.TokenResponse.accessToken;
                headers["chatId"] = chatId;

                WS.CustomHeaders = headers;

                WS.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

                WS.ConnectAsync();
            }
            catch (Exception ex)
            {
                print(ex.ToString());
            }

            WS.OnOpen += (object sender, EventArgs e) => {
                print("соединен");
                SendMessageAsync(message);
            };
        }
    }



    private void SendMessageAsync(SentMessage message)
    {
        var str = SerializeObject(message);

        WS.SendAsync(str, (bool c) =>
        {
            if (c)
            { 
                print("сообщение отправлено");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (message.MessageBody == null) return;

                    _chatView.Data.InsertOneAtEnd(new ChatMessageItemModel()
                    {
                        Message = new MessageBody()
                        {
                            Content = _messageField.Text,
                            Date = DateTime.Now,
                            Type = ChatMessageType.Text
                        },
                        IsMine = true
                    });
                    _chatView.SetNormalizedPosition(0);
                    _messageField.SetText("");
                    _proposal.SetActive(false);
                });

            }
            else
            {
                print("сообщение не отправлено");
            }
        });
    }



    public void OnKeyboardHeightChanged(int keyboardHeight)
    {
        Debug.Log("OnKeyboardHeightChanged: " + keyboardHeight);
        //_chatView.UpdateKeyboardHeight(keyboardHeight);
        //_chatView.UpdateChatHistorySize();
    }



    public async void GenerateMessageList()
    {


        var ms = await ServerConstants.Instance.GetChatMessagesAsync(_chatInfo.id, 30);

        if (ms.Count > 0) _proposal.SetActive(false);
        else _proposal.SetActive(true);

        for (int i = ms.Count - 1; i >= 0; i--)
        {
            string senderMail;
            foreach (var p in _chatInfo.participants)
            {
                if (p.id == ms[i].SenderId.ToString())
                {
                    if(p.identifier == AccountManager.Instance.ProfileData.identifier)
                    {
                        _chatView.Data.InsertOneAtEnd(new ChatMessageItemModel() 
                        { 
                            Message = ms[i],
                            IsMine = true
                        });
                    }
                    else
                    {
                        senderMail = p.identifier;
                        _chatView.Data.InsertOneAtEnd(new ChatMessageItemModel()
                        {
                            Message = ms[i],
                            IsMine = false
                        });
                    }
                }
            }
        }
        _chatView.SetNormalizedPosition(0);

        var message = new SentMessage
        {
            MessageBody = null,
            LastMessageReadId = ms[0].Id
        };

        SendMessageAsync(message);

        //foreach (var m in _messagesFromDB)
        //{
        //    if (m.SenderId == AccountManager.Instance.ProfileData.id)
        //    {
        //        _chatView.AddMessageRight(m.Content);
        //    }
        //    else
        //    {
        //        string senderMail = "";
        //        foreach (var p in _chatInfo.participants)
        //        {
        //            if (p.id == m.SenderId.ToString())
        //            {
        //                senderMail = p.email;
        //            }
        //        }
        //        _chatView.AddMessageLeft(m.Content, senderMail);
        //    }
        //}
    }



    private string SerializeObject<T>(T obj) => JsonConvert.SerializeObject(obj);
}

