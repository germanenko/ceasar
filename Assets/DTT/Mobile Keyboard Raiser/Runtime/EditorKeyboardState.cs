#if UNITY_EDITOR
using System;
using UnityEngine;
using Object = System.Object;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Debug implementation of <see cref="IKeyboardState"/> for managing an in editor keyboard.
    /// </summary>
    public class EditorKeyboardState : IKeyboardState
    {
        /// <summary>
        /// The height in pixels of the keyboard on-screen.
        /// </summary>
        public int PixelHeight => (int)EditorKeyboard.Area.height;
        
        /// <summary>
        /// The proportional height of the keyboard compared to the screen height.
        /// </summary>
        public float ProportionalHeight => EditorKeyboard.Area.height / Screen.height;
        
        /// <summary>
        /// Whether the keyboard is currently raised.
        /// </summary>
        public bool IsRaised => EditorKeyboard.IsActive;
        
        /// <summary>
        /// Called when the keyboard is raised.
        /// </summary>
        public event Action Raised;
        
        /// <summary>
        /// Called when the keyboard is lowered.
        /// </summary>
        public event Action Lowered;

        /// <summary>
        /// Add event handlers.
        /// </summary>
        public EditorKeyboardState()
        {
            EditorKeyboard.Opened += () => Raised?.Invoke();
            EditorKeyboard.Closed += () => Lowered?.Invoke();
        }
    }
}
#endif