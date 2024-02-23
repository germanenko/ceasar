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
    [SerializeField] private TextMeshProUGUI _unreadMessagesText;

    [SerializeField] private GameObject _unreadMessages;

    [SerializeField] private ChatListManager _chatList;

    [SerializeField] private Color _defaultLastMessageColor;

    public void Init(TaskChatBody chat, ChatListManager chatList)
    {
        _defaultLastMessageColor = _lastMessage.color;

        _chatInfo = chat;
        _chatList = chatList;

        SetInfo();
    }



    public void UpdateLastMessage(string lastMessage, DateTime lastMessageTime)
    {
        if (lastMessage.Length <= 21)
        {
            _lastMessage.text = lastMessage;
        }
        else
        {
            _lastMessage.text = lastMessage.Substring(0, 17) + "...";
        }

        if (_chatInfo.lastMessage != null)
        {
            if (_chatInfo.lastMessage.Date.Date != DateTime.Now.Date)
            {
                _lastMessageDate.text = lastMessageTime.Date.ToShortDateString();
            }
            else
            {
                _lastMessageDate.text = $"{lastMessageTime.ToLocalTime().Hour}:{lastMessageTime.ToLocalTime().Minute}";
            }
        }
        else
        {
            _chatInfo.lastMessage = new MessageBody();

            if (lastMessageTime.Date != DateTime.Now.Date)
            {
                _lastMessageDate.text = lastMessageTime.Date.ToShortDateString();
            }
            else
            {
                _lastMessageDate.text = $"{lastMessageTime.ToLocalTime().Hour}:{lastMessageTime.ToLocalTime().Minute}";
            }
        }
    }



    public void SetInfo()
    {
        _chatName.text = _chatInfo.name;

        if(_chatInfo.lastMessage != null)
        {
            if(_chatInfo.lastMessage.Content.Length <= 21)
            {
                _lastMessage.text = _chatInfo.lastMessage.Content;
            }
            else
            {
                _lastMessage.text = _chatInfo.lastMessage.Content.Substring(0, 17) + "...";
            }

            if (_chatInfo.lastMessage.Date.Date != DateTime.Now.Date)
            {
                _lastMessageDate.text = _chatInfo.lastMessage.Date.Date.ToString();
            }
            else
            {
                _lastMessageDate.text = _chatInfo.lastMessage.Date.ToShortTimeString();
            }
        }   
    }



    public void OpenChat()
    {
        _chatList.OpenChat(_chatInfo);
        SetLastMessageBold(false);
    }



    public void SetLastMessageBold(bool bold)
    {
        _lastMessage.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        _lastMessage.color = bold ? Color.black : _defaultLastMessageColor;
    }

    

    public void SetUnreadMessages(int count)
    {
        _unreadMessages.SetActive(true);
        _unreadMessagesText.text = count.ToString();
    }
}
