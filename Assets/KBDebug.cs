using DTT.KeyboardRaiser;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KBDebug : MonoBehaviour
{
    public UIKeyboardRaiser KeyboardRaiser;
    public TextMeshProUGUI View;

    private void Start()
    {
        KeyboardRaiser.KeyboardState.Raised += SetTextOpened;
        KeyboardRaiser.KeyboardState.Lowered += SetTextClosed;
    }

    public void SetTextOpened()
    {
        View.text = "клава открыта";
    }
    public void SetTextClosed()
    {
        View.text = "клава скрыта";
    }
}
