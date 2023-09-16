using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Manages the keyboard states for the different platforms and exposes the relevant ones.
    /// </summary>
    public static class KeyboardStateManager
    {
        /// <summary>
        /// The keyboard state that is used by this platform.
        /// </summary>
        public static IKeyboardState Current { get; }
        public static List<UIKeyboardRaiser> openingField = new List<UIKeyboardRaiser>();

        /// <summary>
        /// Initializes the keyboard state to be used.
        /// </summary>
        static KeyboardStateManager()
        {
            Current =
#if UNITY_EDITOR
                new EditorKeyboardState();
#elif UNITY_ANDROID
                new AndroidKeyboardState(Updater.Instance, false);
#elif UNITY_IOS
                new IOSKeyboardState(Updater.Instance);
#else
                new NullKeyboardState();
                Debug.LogWarning("Unsupported platform for Keyboard Raiser.");
#endif
        }
    }
}