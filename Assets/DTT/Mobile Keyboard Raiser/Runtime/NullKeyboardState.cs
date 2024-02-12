using System;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Null-object pattern implementation of the <see cref="IKeyboardState"/>.
    /// <a href="https://refactoring.guru/introduce-null-object">Null-object</a>
    /// </summary>
    public sealed class NullKeyboardState : IKeyboardState
    {
        /// <summary>
        /// Default height of zero.
        /// </summary>
        public int PixelHeight => 0;
        
        /// <summary>
        /// Default height of zero.
        /// </summary>
        public float ProportionalHeight => 0;
        
        /// <summary>
        /// Defaulted to not being raised.
        /// </summary>
        public bool IsRaised => false;
        
        /// <summary>
        /// Blank implementation. (Never invoked)
        /// </summary>
        #pragma warning disable CS0067
        public event Action Raised;
        
        /// <summary>
        /// Blank implementation. (Never invoked)
        /// </summary>
        #pragma warning disable CS0067
        public event Action Lowered;
        public event Action OnInputFieldChanged;
    }
}