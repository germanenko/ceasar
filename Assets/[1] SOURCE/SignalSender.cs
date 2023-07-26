using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Framework;
using TMPro;



namespace Germanenko.Source
{



    public class SignalSender : MonoBehaviour
    {


        public void SendSignalEndInputDD()
        {
            Signals.Send(new SignalEndInput { sender = transform.parent.parent.parent });
        }



        public void SendSignalEndInput()
        {
            Signals.Send(new SignalEndInput { sender = transform });
        }

    }

}
