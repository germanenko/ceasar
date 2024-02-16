using AdvancedInputFieldPlugin;
using AdvancedInputFieldSamples;
using Doozy.Runtime.UIManager.Containers;
using Germanenko.Framework;
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

    [SerializeField] private WebSocket WS;

    [SerializeField] private AdvancedInputField _messageField;

    [SerializeField] private ChatListManager _chatListManager;

    [SerializeField] private UIView _view;

    [SerializeField] private ChatView _chatView;

    [SerializeField] private TextMeshProUGUI _chatNameText;

    public void OpenChat(TaskChatBody chatInfo, WebSocket ws)
    {
        _chatInfo = chatInfo;

        WS = ws;

        _view.Show();

        _chatNameText.text = _chatInfo.name;

        WS.OnMessage += WS_OnMessage;

#if (UNITY_ANDROID || UNITY_IOS)
        if (!Application.isEditor || Settings.SimulateMobileBehaviourInEditor)
        {
            NativeKeyboardManager.AddKeyboardHeightChangedListener(OnKeyboardHeightChanged);
        }
#endif
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
                senderMail = p.email;
            }
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() => _chatView.AddMessageLeft(msg.Content, senderMail));
        print($"Получено сообщение от {senderMail}: {msg.Content}");
    }

    public void SendMessage()
    {
        var messageBody = new CreateMessageBody
        {
            Type = ChatMessageType.Text,
            Content = _messageField.Text,
        };
        var str = SerializeObject(messageBody);
        //WS.Send(str);
        WS.SendAsync(str, (bool c) =>
        {
            if (c)
            {
                print("сообщение отправлено");
                UnityMainThreadDispatcher.Instance().Enqueue(() => _chatView.AddMessageRight(_messageField.Text));
                _messageField.SetText("");

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
        _chatView.UpdateKeyboardHeight(keyboardHeight);
        _chatView.UpdateChatHistorySize();
    }

    private string SerializeObject<T>(T obj) => JsonConvert.SerializeObject(obj);
}



public class MessageBody
{
    public Guid Id { get; set; }
    public ChatMessageType Type { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public Guid SenderId { get; set; }
}



public class CreateMessageBody
{
    public ChatMessageType Type { get; set; }

    public string Content { get; set; }

    public override string ToString()
    {
        return $"Content: {Content} sending";
    }
}

public enum ChatMessageType
{
    Text,
    File
}
