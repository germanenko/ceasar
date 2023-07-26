#if UNITY_IOS
using System;
using UnityEngine;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Manages keyboard state for IOS platform.
    /// </summary>
    public class IOSKeyboardState : IKeyboardState
    {
        /// <summary>
        /// The height in pixels of the on-screen keyboard.
        /// </summary>
        public int PixelHeight => (int)TouchScreenKeyboard.area.height;
        
        /// <summary>
        /// The percentage of height the on-screen keyboard covers in a range of zero to one (0..1).
        /// </summary>
        public float ProportionalHeight => PixelHeight / (float)Screen.height;

        /// <summary>
        /// Whether the keyboard is raised or not.
        /// </summary>
        public bool IsRaised => _raised;

        /// <summary>
        /// Is called whenever the on-screen keyboard is appears.
        /// </summary>
        public event Action Raised;
        
        /// <summary>
        /// Is called whenever the on-screen keyboard is disappears.
        /// </summary>
        public event Action Lowered;
        
        /// <summary>
        /// Used for gaining Unity update loop callbacks without needing a <see cref="MonoBehaviour"/> implementation.
        /// </summary>
        private readonly IUpdater _updater;
        
        /// <summary>
        /// Contains whether the screen was raised last frame,
        /// so we can compare it to the current and detect the moment of change.
        /// </summary>
        private bool _raised = false;

        /// <summary>
        /// Creates a new keyboard state manager for IOS.
        /// </summary>
        /// <param name="updater">
        /// Used for gaining Unity update loop callbacks
        /// without needing a <see cref="MonoBehaviour"/> implementation.
        /// </param>
        public IOSKeyboardState(IUpdater updater)
        {
            _updater = updater;
            _updater.Updated += OnUpdate;
        }
        
        /// <summary>
        /// Polls whether the on-screen keyboard is visible to detect the moment of change to fire an event.
        /// </summary>
        private void OnUpdate()
        {
            if (TouchScreenKeyboard.visible && !_raised)
            {
                _raised = true;
                Raised?.Invoke();
            }
            else if (!TouchScreenKeyboard.visible && _raised)
            {
                _raised = false;
                Lowered?.Invoke();
            }
        }
    }
}
#endif
