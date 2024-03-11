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
using WebSocketSharp;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TaskChatBody _chatInfo;

    [SerializeField] private WebSocket WS = new WebSocket("wss://itsmydomain.ru/chat");

    [SerializeField] private AdvancedInputField _messageField;

    [SerializeField] private ChatListManager _chatListManager;

    [SerializeField] private UIView _view;

    [SerializeField] private ChatView _chatView;

    [SerializeField] private TextMeshProUGUI _chatNameText;

    [SerializeField] private List<ChatMessages> _messagesFromDB;

    [SerializeField] private GameObject _proposal;

    public void OpenChat(TaskChatBody chatInfo, WebSocket ws)
    {
        _chatInfo = chatInfo;

        WS = ws;

        _view.Show();

        _chatNameText.text = _chatInfo.name;

        //_messagesFromDB = Toolbox.Get<Tables>().GetMessages(_chatInfo.id, 30);

        _proposal.SetActive(_messagesFromDB.Count == 0);

        GenerateMessageList();

        WS.OnMessage += WS_OnMessage;
    }


    public void OpenChat(TaskChatBody chatInfo)
    {
        _chatInfo = chatInfo;

        _view.Show();

        _chatNameText.text = _chatInfo.name;

        GenerateMessageList();
    }



    public void CloseChat()
    { 
        print("close");

        _view.Hide();

        WS.Close();
    }



    private void WS_OnMessage(object sender, MessageEventArgs e)
    {
        var msg = JsonConvert.DeserializeObject<MessageBody>(e.Data);
        string senderMail = "";
        foreach (var p in _chatInfo.participants)
        {
            if(p.id == msg.SenderId.ToString())
            {
                senderMail = p.userTag;
            }
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() => _chatView.AddMessageLeft(msg.Content, senderMail));
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

        var str = SerializeObject(message);
        //WS.Send(str);
        if(WS.ReadyState == WebSocketState.Open)
        {
            WS.SendAsync(str, (bool c) =>
            {
                if (c)
                {
                    print("сообщение отправлено");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        _chatView.AddMessageRight(_messageField.Text);
                        _messageField.SetText("");
                    });

                }
                else
                {
                    print("сообщение не отправлено");
                }
            });
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
                WS.SendAsync(str, (bool c) =>
                {
                    if (c)
                    {
                        print("сообщение отправлено");
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            _chatView.AddMessageRight(_messageField.Text);
                            _messageField.SetText("");
                        });

                    }
                    else
                    {
                        print("сообщение не отправлено");
                    }
                });
            };
        }
    }



    public void OnKeyboardHeightChanged(int keyboardHeight)
    {
        Debug.Log("OnKeyboardHeightChanged: " + keyboardHeight);
        _chatView.UpdateKeyboardHeight(keyboardHeight);
        _chatView.UpdateChatHistorySize();
    }



    public void GenerateMessageList()
    {
        for (int i = _messagesFromDB.Count - 1; i >= 0; i--)
        {
            string senderMail;
            foreach (var p in _chatInfo.participants)
            {
                if (p.id == _messagesFromDB[i].SenderId.ToString())
                {
                    if(p.identifier == AccountManager.Instance.ProfileData.identifier)
                    {
                        _chatView.AddMessageRight(_messagesFromDB[i].Content);
                    }
                    else
                    {
                        senderMail = p.identifier;
                        _chatView.AddMessageLeft(_messagesFromDB[i].Content, senderMail);
                    }
                }
            }
        }
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

