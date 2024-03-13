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

    [SerializeField] private RawImage _chatIcon;

    [SerializeField] private TextMeshProUGUI _chatName;
    [SerializeField] private TextMeshProUGUI _lastMessage;
    [SerializeField] private TextMeshProUGUI _lastMessageDate;
    [SerializeField] private TextMeshProUGUI _unreadMessagesText;

    [SerializeField] private GameObject _unreadMessages;

    [SerializeField] private ChatListManager _chatList;
    [SerializeField] private ChatManager _chatManager;

    [SerializeField] private Color _defaultLastMessageColor;

    public void Init(TaskChatBody chat, ChatManager chatManager)
    {
        _defaultLastMessageColor = _lastMessage.color;

        _chatInfo = chat;
        _chatManager = chatManager;

        SetInfo();
    }



    public void UpdateLastMessage(string lastMessage, DateTime lastMessageTime)
    {
        if (lastMessage.Length <= 24)
        {
            _lastMessage.text = lastMessage;
        }
        else
        {
            _lastMessage.text = lastMessage.Substring(0, 20) + "...";
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
        foreach (var participant in _chatInfo.participants) 
        { 
            if(AccountManager.Instance.ProfileData.identifier != participant.identifier)
            {
                _chatName.text = participant.nickname;
                _chatInfo.name = participant.nickname;
            }
        }

        if(_chatInfo.lastMessage != null)
        {
            SetLastMessageBold(true);

            UpdateLastMessage(_chatInfo.lastMessage.Content, _chatInfo.lastMessage.Date);
        }

        string imageUrl = "";

        foreach (var participant in _chatInfo.participants)
        {
            if(participant.identifier != AccountManager.Instance.ProfileData.identifier)
            {
                imageUrl = participant.imageUrl;
            }
        }

        ServerConstants.Instance.StartCoroutine(ServerConstants.Instance.DownloadChatIconTexture((tex) =>
        {
            _chatIcon.texture = tex;
            _chatIcon.color = Color.white;
        },
        imageUrl));
    }



    public void OpenChat()
    {
        _chatManager.OpenChat(_chatInfo, _chatInfo.id == "");
        SetLastMessageBold(false);
    }



    public void SetLastMessageBold(bool bold)
    {
        _lastMessage.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        _lastMessage.color = bold ? Color.black : _defaultLastMessageColor;
    }

    

    public void SetUnreadMessages(int count)
    {
        _unreadMessages.SetActive(count > 0);
        _unreadMessagesText.text = count.ToString();
    }
}
