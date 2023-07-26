#if UNITY_IOS
namespace DTT.KeyboardRaiser.Tests
{
    /// <summary>
    /// Used for testing the iOS implementation of the keyboard state.
    /// </summary>
    public class IOSKeyboardStateTests : KeyboardStateTests<IOSKeyboardState>
    {
        /// <summary>
        /// Returns an instance of <see cref="IOSKeyboardState"/>.
        /// </summary>
        protected override IOSKeyboardState GetKeyboardState => new IOSKeyboardState(Updater.Instance);
        
        /// <summary>
        /// Returns the by iOS supported platforms.
        /// </summary>
        protected override ReadOnlyCollection<RuntimePlatform> SupportedPlatforms => Array.AsReadOnly(new [] { RuntimePlatform.IPhonePlayer });
    }
}
#endif
