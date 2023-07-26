#if UNITY_ANDROID
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace DTT.KeyboardRaiser.Tests
{
    /// <summary>
    /// Used for testing the Android implementation of the keyboard state.
    /// </summary>
    public class AndroidKeyboardStateTests : KeyboardStateTests<AndroidKeyboardState>
    {
        /// <summary>
        /// Returns an instance of <see cref="AndroidKeyboardState"/>.
        /// </summary>
        protected override AndroidKeyboardState GetKeyboardState => new AndroidKeyboardState(Updater.Instance, false);
        
        /// <summary>
        /// Returns the by Android supported platforms.
        /// </summary>
        protected override ReadOnlyCollection<RuntimePlatform> SupportedPlatforms => Array.AsReadOnly(new [] { RuntimePlatform.Android });
    }
}
#endif
