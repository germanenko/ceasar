using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using Doozy.Runtime.Signals;
using UnityEngine.EventSystems;

namespace Germanenko.Source
{

    public class TouchInArea : MonoBehaviour
    {

        [SerializeField] private bool isTapGesture = false;
        [SerializeField] private bool isDoubleTapGesture = false;
        [SerializeField] private bool isTripleTapGesture = false;
        [SerializeField] private bool isLongPressGesture = false;
        [SerializeField] private bool isSwipeHorizontalGesture = false;
        [SerializeField] private bool isSwipeVerticalGesture = false;

        public bool isHandle = false;

        [SerializeField] private bool isDebugging = false;
        [SerializeField] private GameObject touchArea;
        [SerializeField] private string streamCategory;

        private TapGestureRecognizer tapGesture;
        private TapGestureRecognizer doubleTapGesture;
        private TapGestureRecognizer tripleTapGesture;
        private SwipeGestureRecognizer swipeGesture;
        //private PanGestureRecognizer panGesture;
        //private ScaleGestureRecognizer scaleGesture;
        //private RotateGestureRecognizer rotateGesture;
        private LongPressGestureRecognizer longPressGesture;

        public static TouchInArea HorizontalTouch;

        #region TAPS

        private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        {

            if (!isHandle) return;

            if (gesture.State == GestureRecognizerState.Ended)
            {
                DebugText("Tapped at {0}, {1} {2}", gesture.FocusX, gesture.FocusY, streamCategory);
                Signal.Send(streamCategory, "Tap");
            }
        }



        private void CreateTapGesture()
        {

            tapGesture = new TapGestureRecognizer();
            tapGesture.StateUpdated += TapGestureCallback;
            tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
            tapGesture.PlatformSpecificView = touchArea;

            FingersScript.Instance.AddGesture(tapGesture);

        }



        private void DoubleTapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        {

            if (!isHandle) return;

            if (gesture.State == GestureRecognizerState.Ended)
            {
                DebugText("Double tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
                Signal.Send(streamCategory, "DoubleTap");
            }

        }



        private void CreateDoubleTapGesture()
        {

            doubleTapGesture = new TapGestureRecognizer();
            doubleTapGesture.NumberOfTapsRequired = 2;
            doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
            doubleTapGesture.RequireGestureRecognizerToFail = tripleTapGesture;
            doubleTapGesture.PlatformSpecificView = touchArea;

            FingersScript.Instance.AddGesture(doubleTapGesture);

        }



        private void TripleTapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        {

            if (!isHandle) return;

            if (gesture.State == GestureRecognizerState.Ended)
            {
                DebugText("Triple tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
                Signal.Send(streamCategory, "TripleTap");
            }
        }



        private void CreateTripleTapGesture()
        {

            tripleTapGesture = new TapGestureRecognizer();
            tripleTapGesture.NumberOfTapsRequired = 3;
            tripleTapGesture.StateUpdated += TripleTapGestureCallback;
            tripleTapGesture.PlatformSpecificView = touchArea;

            FingersScript.Instance.AddGesture(tripleTapGesture);

        }

        #endregion



        #region LONG PRESS

        private void LongPressGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Began)
            {
                DebugText("Long press began: {0}, {1}", gesture.FocusX, gesture.FocusY);
                //BeginDrag(gesture.FocusX, gesture.FocusY);
            }
            else if (gesture.State == GestureRecognizerState.Executing)
            {
                DebugText("Long press moved: {0}, {1}", gesture.FocusX, gesture.FocusY);
                //DragTo(gesture.FocusX, gesture.FocusY);
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                DebugText("Long press end: {0}, {1}, delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);
                //EndDrag(longPressGesture.VelocityX, longPressGesture.VelocityY);
            }
        }



        private void CreateLongPressGesture()
        {
            longPressGesture = new LongPressGestureRecognizer();
            longPressGesture.MaximumNumberOfTouchesToTrack = 1;
            longPressGesture.StateUpdated += LongPressGestureCallback;
            longPressGesture.PlatformSpecificView = touchArea;

            FingersScript.Instance.AddGesture(longPressGesture);
        }

        #endregion



        //#region TWO FINGERS

        //private void PanGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        //{
        //    if (gesture.State == GestureRecognizerState.Executing)
        //    {
        //        DebugText("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);

        //        //float deltaX = panGesture.DeltaX / 25.0f;
        //        //float deltaY = panGesture.DeltaY / 25.0f;
        //        //Vector3 pos = Earth.transform.position;
        //        //pos.x += deltaX;
        //        //pos.y += deltaY;
        //        //Earth.transform.position = pos;
        //    }
        //}



        //private void CreatePanGesture()
        //{
        //    panGesture = new PanGestureRecognizer();
        //    panGesture.MinimumNumberOfTouchesToTrack = 2;
        //    panGesture.StateUpdated += PanGestureCallback;
        //    FingersScript.Instance.AddGesture(panGesture);
        //}



        //private void ScaleGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        //{
        //    if (gesture.State == GestureRecognizerState.Executing)
        //    {
        //        DebugText("Scaled: {0}, Focus: {1}, {2}", scaleGesture.ScaleMultiplier, scaleGesture.FocusX, scaleGesture.FocusY);
        //        //Earth.transform.localScale *= scaleGesture.ScaleMultiplier;
        //    }
        //}



        //private void CreateScaleGesture()
        //{
        //    scaleGesture = new ScaleGestureRecognizer();
        //    scaleGesture.StateUpdated += ScaleGestureCallback;
        //    FingersScript.Instance.AddGesture(scaleGesture);
        //}



        //private void RotateGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        //{
        //    if (gesture.State == GestureRecognizerState.Executing)
        //    {
        //        DebugText("Rotates: {0}, Focus: {1}, {2}", rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg, scaleGesture.FocusX, scaleGesture.FocusY);
        //        //Earth.transform.Rotate(0.0f, 0.0f, rotateGesture.RotationRadiansDelta * Mathf.Rad2Deg);
        //    }
        //}



        //private void CreateRotateGesture()
        //{
        //    rotateGesture = new RotateGestureRecognizer();
        //    rotateGesture.StateUpdated += RotateGestureCallback;
        //    FingersScript.Instance.AddGesture(rotateGesture);
        //}

        //#endregion



        #region SWIPE

        private void SwipeGestureCallback(GestureRecognizer gesture)
        {

            if (!isHandle) return;

            if (gesture.State == GestureRecognizerState.Ended)
            {
                DebugText("Swiped from {0},{1} to {2},{3}, dir {4}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.EndDirection);

                if (isSwipeHorizontalGesture)
                {
                    if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Left) Signal.Send(streamCategory, "Left");
                    else if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Right) Signal.Send(streamCategory, "Right");
                }

                if (isSwipeVerticalGesture)
                {
                    if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Up) Signal.Send(streamCategory, "Up");
                    else if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Down) Signal.Send(streamCategory, "Down");
                }

            }

        }



        private void CreateSwipeGesture()
        {

            swipeGesture = new SwipeGestureRecognizer();
            swipeGesture.StateUpdated += SwipeGestureCallback;
            swipeGesture.DirectionThreshold = 2.0f; // allow a swipe, regardless of slope
            swipeGesture.PlatformSpecificView = touchArea;

            FingersScript.Instance.AddGesture(swipeGesture);

        }

        private void OnEnable()
        {
            swipeGesture.StateUpdated += SwipeGestureCallback;
            FingersScript.Instance.AddGesture(swipeGesture);
        }
        private void OnDisable()
        {
            if(FingersScript.HasInstance)
            {
                swipeGesture.StateUpdated -= SwipeGestureCallback;
                FingersScript.Instance.RemoveGesture(swipeGesture);
            }  
        }

        #endregion



        public void IsHandle(bool isHandle)
        {
            this.isHandle = isHandle;
        }



        private void Awake()
        {
            HorizontalTouch = this;

            if (isTripleTapGesture) CreateTripleTapGesture();
            if(isDoubleTapGesture) CreateDoubleTapGesture();
            if(isTapGesture) CreateTapGesture();

            //CreateLongPressGesture();

            //CreatePanGesture();
            //CreateScaleGesture();
            //CreateRotateGesture();

            if(isSwipeHorizontalGesture || isSwipeVerticalGesture) CreateSwipeGesture();

        }



        private void DebugText(string text, params object[] format)
        {
            if(isDebugging)
               Debug.Log(string.Format(text, format));
        }


    }

}