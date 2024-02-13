using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private TaskChatBody _chatInfo;

    [SerializeField] private Image _chatIcon;

    [SerializeField] private TextMeshProUGUI _chatName;
    [SerializeField] private TextMeshProUGUI _lastMessage;

    [SerializeField] private ChatListManager _chatList;

    public void Init(TaskChatBody chat, ChatListManager chatList)
    {
        _chatInfo = chat;
        _chatList = chatList;

        SetInfo();
    }

    

    public void SetInfo()
    {
        _chatName.text = _chatInfo.name;
    }



    public void OpenChat()
    {
        _chatList.OpenChat(_chatInfo);
    }
}
