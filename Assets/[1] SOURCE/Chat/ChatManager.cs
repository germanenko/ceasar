using AdvancedInputFieldPlugin;
using Doozy.Runtime.UIManager.Containers;
using Germanenko.Framework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TaskChatBody _chatInfo;

    [SerializeField] private AdvancedInputField _messageField;

    [SerializeField] private ChatListManager _chatListManager;

    [SerializeField] private UIView _view;

    public void OpenChat(TaskChatBody chatInfo)
    {
        _chatInfo = chatInfo;

        _view.Show();

        //Task.Run(() => ChatUpdate());
    }

    //public async Task ChatUpdate()
    //{
    //    if (_chatListManager.WS.State == WebSocketState.Open)
    //    {
    //        var receiveTask = ReceiveMessage();
    //        var delayTask = Task.Delay(3000);
    //        var completedTask = await Task.WhenAny(receiveTask, delayTask);
    //        if (completedTask == receiveTask)
    //        {
    //            var receivedMessage = receiveTask.Result;
    //            print($"Получено сообщение: {receivedMessage.Content} by {name}");
    //        }
    //        else
    //            print("сбой");
    //    }
    //}



    public async void SendMessage()
    {
        var messageBody = new CreateMessageBody
        {
            Type = ChatMessageType.Text,
            Content = _messageField.Text,
        };
        var str = SerializeObject(messageBody);
        _chatListManager.WS.Send(str);
    }



    //async Task SendMessageAsync(string message)
    //{
    //    var messageBytes = Encoding.UTF8.GetBytes(message);
    //    await _chatListManager.WS.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
    //}



    //async Task<MessageBody?> ReceiveMessage()
    //{
    //    var receiveBuffer = new byte[1024];
    //    var result = await _chatListManager.WS.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

    //    if (result.MessageType == WebSocketMessageType.Text)
    //    {
    //        var str = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
    //        return JsonConvert.DeserializeObject<MessageBody>(str);
    //    }

    //    return null;
    //}



    //private async Task<MemoryStream?> ReceiveMessage(CancellationToken token)
    //{
    //    byte[] bytes = new byte[4096];
    //    WebSocketReceiveResult? receiveResult = null;
    //    MemoryStream stream = new();

    //    do
    //    {
    //        receiveResult = await _chatListManager.WS.ReceiveAsync(bytes, token);
    //        if (receiveResult.MessageType == WebSocketMessageType.Close && _chatListManager.WS.State != WebSocketState.Closed)
    //        {
    //            await _chatListManager.WS.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, token);
    //            return null;
    //        }
    //        else if (receiveResult.Count > 0)
    //            stream.Write(bytes, 0, receiveResult.Count);
    //    } while (!receiveResult.EndOfMessage && _chatListManager.WS.State == WebSocketState.Open);

    //    return stream;
    //}

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
