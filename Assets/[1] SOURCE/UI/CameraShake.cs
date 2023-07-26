using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Germanenko.Framework;
using DG.Tweening;



namespace Germanenko.Source
{

    public class CameraShake : IReceive<SignalCameraShake>
    {

        private Transform cameraTransform;


        public CameraShake() 
        {
            Signals.Add(this);
        }



        private void twShake(int strength)
        {

            if (cameraTransform == null)
                cameraTransform = ConstantSingleton.Instance.MainCamera.gameObject.transform;


            CorrectCamera();
            var twShake = DOTween.Sequence()
                .Append(cameraTransform.DOShakeRotation(0.7f * strength, 1.3f * strength))
                .OnComplete(CorrectCamera)
                .OnPlay(CorrectCamera);



        }



        public void CorrectCamera()
        {
            cameraTransform.rotation = Quaternion.identity;
        }



        public void HandleSignal(SignalCameraShake arg)
        {

            twShake(arg.strength);

        }
        
        
        
        ~CameraShake()
        {
            Signals.Remove(this);
        }

    }

}
