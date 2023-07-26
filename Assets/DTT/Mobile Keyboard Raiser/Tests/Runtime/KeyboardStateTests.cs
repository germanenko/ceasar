using System;
using System.Collections;
using System.Collections.ObjectModel;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DTT.KeyboardRaiser.Tests
{
    /// <summary>
    /// Base class for testing keyboard state implementations.
    /// </summary>
    /// <typeparam name="T">The type of keyboard state that is being tested.</typeparam>
    public abstract class KeyboardStateTests<T> where T : IKeyboardState
    {
        /// <summary>
        /// Should retrieve the keyboard state that's being tested.
        /// </summary>
        protected abstract T GetKeyboardState { get; }
        
        /// <summary>
        /// The platforms that are supported for this test.
        /// </summary>
        protected abstract ReadOnlyCollection<RuntimePlatform> SupportedPlatforms { get; }
        
        /// <summary>
        /// Does a check for the supported platforms. Since on-screen keyboards are platform specific.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if current platform is not supported.</exception>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Check supported platforms.
            ReadOnlyCollection<RuntimePlatform> supported = SupportedPlatforms;
            for (int i = 0; i < supported.Count; i++)
                if (Application.platform == supported[i]) 
                    return;
            throw new NotSupportedException($"These tests are not supported for the {Application.platform} platform.");
        }
        
        /// <summary>
        /// Tests that the keyboard pixel height is higher than zero when the keyboard is active.
        /// Expects the result to be higher than zero.
        /// <remarks>We can't expect the keyboard height since this can vary per phone.</remarks>
        /// </summary>
        [UnityTest]
        public IEnumerator PixelHeight_KeyboardActive_GreaterThanZero()
        {
            // Arrange.
            IKeyboardState state = GetKeyboardState;

            // Act.
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.Greater(state.PixelHeight, 0);
            
            // Clean up.
            keyboard.active = false;
            yield return new WaitForSeconds(.5f);
        }
        
        /// <summary>
        /// Tests that when the keyboard is not active the height is equal to zero.
        /// Expects zero since there is no keyboard active.
        /// </summary>
        [Test]
        public void PixelHeight_KeyboardInactive_IsZero()
        {
            // Arrange.
            IKeyboardState state = GetKeyboardState;
            
            // Assert.
            Assert.Zero(state.PixelHeight);
        }
        
        /// <summary>
        /// Tests whether the raised flag returns false when the keyboard isn't active.
        /// Expects the flag to be false if no keyboard is active.
        /// </summary>
        [Test]
        public void IsRaised_KeyboardInactive_IsFalse()
        {
            // Arrange.
            IKeyboardState state = GetKeyboardState;
            
            // Assert.
            Assert.IsFalse(state.IsRaised);
        }
        
        /// <summary>
        /// Tests whether the raised flag returns true when the keyboard is active.
        /// Expects the flag to be true if the keyboard is active.
        /// </summary>
        [UnityTest]
        public IEnumerator IsRaised_KeyboardActive_IsTrue()
        {
            // Arrange.
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            IKeyboardState state = GetKeyboardState;

            // Act.
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.IsTrue(state.IsRaised);
            
            // Clean up.
            keyboard.active = false;
            yield return new WaitForSeconds(.5f);
        }
        
        /// <summary>
        /// Tests whether the raised event is invoked when the keyboard becomes active.
        /// Expects the event to invoked when the keyboard is opened.
        /// </summary>
        [UnityTest]
        public IEnumerator RaisedEvent_KeyboardSetActive_Invokes()
        {
            // Arrange.
            bool invoked = false;
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            IKeyboardState state = GetKeyboardState;
            state.Raised += OnRaised;
            
            // Act.
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.IsTrue(invoked);
            
            // Clean up.
            keyboard.active = false;
            state.Raised -= OnRaised;
            yield return new WaitForSeconds(.5f);

            void OnRaised() => invoked = true;
        }
        
        /// <summary>
        /// Tests whether the raised event is not invoked when the keyboard becomes inactive.
        /// Expects the event to not be invoked when the keyboard is closed.
        /// </summary>
        [UnityTest]
        public IEnumerator RaisedEvent_KeyboardSetInactive_DoesntInvoke()
        {
            // Arrange.
            bool invoked = false;
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            IKeyboardState state = GetKeyboardState;
            
            // Act.
            yield return new WaitForSeconds(.5f);
            state.Raised += OnRaised;
            
            keyboard.active = false;
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.IsFalse(invoked);
            
            // Clean up.
            keyboard.active = false;
            state.Raised -= OnRaised;
            yield return new WaitForSeconds(.5f);

            void OnRaised() => invoked = true;
        }
        
        /// <summary>
        /// Tests whether the lowered event is invoked when the keyboard is lowered.
        /// Expects the event to fire when the keyboard is closed.
        /// </summary>
        [UnityTest]
        public IEnumerator LoweredEvent_KeyboardSetInactive_Invokes()
        {
            // Arrange.
            bool invoked = false;
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            IKeyboardState state = GetKeyboardState;
            state.Lowered += OnLowered;
            
            // Act.
            yield return new WaitForSeconds(.5f);
            keyboard.active = false;
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.IsTrue(invoked);
            
            // Clean up
            state.Lowered -= OnLowered;
            
            void OnLowered() => invoked = true;
        }
        
        /// <summary>
        /// Tests whether the lowered event isn't fired when the keyboard is opened.
        /// Expects the lowered event not to fire when the keyboard is set active.
        /// </summary>
        [UnityTest]
        public IEnumerator LoweredEvent_KeyboardSetActive_DoesntInvoke()
        {
            // Arrange.
            bool invoked = false;
            TouchScreenKeyboard keyboard = OpenAndRetrieveKeyboard();
            IKeyboardState state = GetKeyboardState;
            state.Lowered += OnLowered;
            
            // Act.
            yield return new WaitForSeconds(.5f);
            
            // Assert.
            Assert.IsFalse(invoked);
            
            // Clean up.
            state.Lowered -= OnLowered;
            keyboard.active = false;

            void OnLowered() => invoked = true;
        }

        /// <summary>
        /// Helper method to open the keyboard and get the reference to it.
        /// </summary>
        /// <returns>The reference to the active keyboard.</returns>
        private TouchScreenKeyboard OpenAndRetrieveKeyboard() => TouchScreenKeyboard.Open(
            String.Empty, 
            TouchScreenKeyboardType.Default, 
            false, 
            false, 
            false, 
            false, 
            string.Empty, 
            int.MaxValue
        );
    }
}