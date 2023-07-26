using Germanenko.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Germanenko.Source
{

    public class SenderSignal : MonoBehaviour
    {



        private void Start()
        {
            Signals.Add(this);
        }



        void OnDestroy()
        {
            Signals.Remove(this);
        }

    }

}
