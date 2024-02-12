using System;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Should be implemented on objects that can perceive the state of the native on-screen keyboard.
    /// </summary>
    public interface IKeyboardState
    {
        /// <summary>
        /// Should return how many pixels the keyboard covers vertically.
        /// </summary>
        int PixelHeight { get; }
        
        /// <summary>
        /// Should return a decimal in zero to one range (0..1) for how much of the screen is covered by the keyboard.
        /// </summary>
        float ProportionalHeight { get; }
        
        /// <summary>
        /// Should return whether the keyboard is raised or not.
        /// </summary>
        bool IsRaised { get; }
        
        /// <summary>
        /// Should be invoked when the keyboard is raised.
        /// </summary>
        event Action Raised;
        
        /// <summary>
        /// Should be invoked when the keyboard is lowered.
        /// </summary>
        event Action Lowered;


        event Action OnInputFieldChanged;

    }
}