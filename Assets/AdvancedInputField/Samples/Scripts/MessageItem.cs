using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _senderText;
    public TextMeshProUGUI SenderText => _senderText;
    [SerializeField] private TextMeshProUGUI _messageText;
    public TextMeshProUGUI MessageText => _messageText;

    public void Init(string  sender, string message)
    {
        _senderText.text = sender; 
        _messageText.text = message;
    }
}
