#if UNITY_ANDROID
using AdvancedInputFieldPlugin;
using NUnit.Framework;
using System;
using UnityEngine;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Manages the on-screen keyboard state for Android platform.
    /// </summary>
    public class AndroidKeyboardState : IKeyboardState, IDisposable
    {
        /// <summary>
        /// Retrieves the height in pixels of the on-screen keyboard.
        /// Returns 0 while keyboard is rising due to a delay when necessary Android events are called.
        /// </summary>

        public int PixelHeight
        {
            get
            {
                _view = _unityPlayer.Call<AndroidJavaObject>("getView");
                _dialog = _unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                if(_view == null || _dialog == null)
                    return 0;

                using(AndroidJavaObject decorView = _dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView"))
                using(AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
                {
                    int decorHeight = 0;
                    if(decorView != null)
                        decorHeight = decorView.Call<int>("getHeight");

                    _view.Call("getWindowVisibleDisplayFrame", rect);

                    int visibleHeight = rect.Call<int>("height");
                    if(visibleHeight == Screen.height)
                        return 0;

                    Debug.Log($"{IsRaised} - {Screen.height} - {visibleHeight} + ({_accountForDecor} ? {decorHeight} : 0) = {Screen.height - visibleHeight + (_accountForDecor ? decorHeight : 0)}");

                    return Screen.height - visibleHeight + (_accountForDecor ? decorHeight : 0);
                }
            }
        }

        /// <summary>
        /// The percentage of height the on-screen keyboard covers in a range of zero to one (0..1).
        /// </summary>
        public float ProportionalHeight => PixelHeight / (float)Screen.height;

        /// <summary>
        /// Whether the keyboard is raised or not.
        /// </summary>
        public bool IsRaised => NativeKeyboardManager.Keyboard.PixelHeight > 0;


        /// <summary>
        /// Is called whenever the on-screen keyboard appears.
        /// </summary>
        public event Action Raised;

        /// <summary>
        /// Is called whenever the on-screen keyboard disappears.
        /// </summary>
        public event Action Lowered;
        public event Action OnInputFieldChanged;

        /// <summary>
        /// The Java Unity class type.
        /// </summary>
        private readonly AndroidJavaClass _unityClass;

        /// <summary>
        /// The Java object for the Unity player.
        /// </summary>
        private readonly AndroidJavaObject _unityPlayer;

        /// <summary>
        /// The Java object for the view.
        /// </summary>
        private AndroidJavaObject _view;

        /// <summary>
        /// The Java object for the activity.
        /// </summary>
        private readonly AndroidJavaObject _currentActivity;

        /// <summary>
        /// The Java object for the dialog.
        /// </summary>
        private AndroidJavaObject _dialog;

        /// <summary>
        /// Used for polling without the need of a <see cref="MonoBehaviour"/> implementation.
        /// </summary>
        private readonly IUpdater _updater;

        /// <summary>
        /// Whether the on-screen keyboard is raised.
        /// </summary>
        private bool _raised = false;

        /// <summary>
        /// Whether to account for the input field above the keyboard.
        /// </summary>
        private bool _accountForDecor;

        /// <summary>
        /// Creates a new keyboard state that manages the on-screen keyboard state for Android platform.
        /// </summary>
        /// <param name="updater">Used for polling without the need of a <see cref="MonoBehaviour"/> implementation.</param>
        /// <param name="accountForDecor">Whether to account for the input field above the keyboard.</param>
        public AndroidKeyboardState(IUpdater updater, bool accountForDecor)
        {
            _accountForDecor = accountForDecor;
            _updater = updater;
            _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _currentActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            _unityPlayer = _currentActivity.Get<AndroidJavaObject>("mUnityPlayer");

            _updater.Updated += OnUpdate;
        }

        /// <summary>
        /// Disposes all the Java objects used for polling the native Keyboard.
        /// </summary>
        public void Dispose()
        {
            _unityClass?.Dispose();
            _unityPlayer?.Dispose();
            _view?.Dispose();
            _currentActivity?.Dispose();
            _dialog?.Dispose();
        }

        /// <summary>
        /// Polls whether the on-screen keyboard is visible to detect the moment of change to fire an event.
        /// </summary>
        private void OnUpdate()
        {
            //if (TouchScreenKeyboard.visible && !_raised)
            //{
            //    _raised = true;
            //    Raised?.Invoke();
            //}
            //else if (!TouchScreenKeyboard.visible && _raised)
            //{
            //    _raised = false;
            //    Lowered?.Invoke();
            //}
            if (NativeKeyboardManager.Keyboard.State == KeyboardState.VISIBLE && !_raised)
            {
                _raised = true;
                Raised?.Invoke();
            }
            else if (NativeKeyboardManager.Keyboard.State != KeyboardState.VISIBLE && _raised)
            {
                _raised = false;
                Lowered?.Invoke();
            }
        }
    }
}
#endif
