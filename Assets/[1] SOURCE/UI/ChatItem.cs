using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private TaskChatBody _chatInfo;
    public TaskChatBody ChatInfo => _chatInfo;

    [SerializeField] private Image _chatIcon;

    [SerializeField] private TextMeshProUGUI _chatName;
    [SerializeField] private TextMeshProUGUI _lastMessage;
    [SerializeField] private TextMeshProUGUI _lastMessageDate;

    [SerializeField] private ChatListManager _chatList;

    public void Init(TaskChatBody chat, ChatListManager chatList)
    {
        _chatInfo = chat;
        _chatList = chatList;

        SetInfo();
    }



    public void UpdateLastMessage(string lastMessage, DateTime lastMessageTime)
    {
        _lastMessage.text = lastMessage;

        print(lastMessageTime.Minute);

        if (_chatInfo.lastMessage.Date.Date != DateTime.Now.Date)
        {
            _lastMessageDate.text = lastMessageTime.Date.ToShortDateString();
        }
        else
        {
            _lastMessageDate.text = $"{lastMessageTime.Hour}:{lastMessageTime.Minute}";
        }
    }



    public void SetInfo()
    {
        _chatName.text = _chatInfo.name;
        _lastMessage.text = _chatInfo.lastMessage.Content;

        if(_chatInfo.lastMessage.Date.Date != DateTime.Now.Date)
        {
            _lastMessageDate.text = _chatInfo.lastMessage.Date.Date.ToString();
        }
        else
        {
            _lastMessageDate.text = _chatInfo.lastMessage.Date.ToShortTimeString();
        }
    }



    public void OpenChat()
    {
        _chatList.OpenChat(_chatInfo);
    }
}
