using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DTT.KeyboardRaiser.Tests
{
    /// <summary>
    /// Tests the UI raising functionalities of the <see cref="UIKeyboardRaiser"/> component.
    /// </summary>
    public class UIKeyboardRaiserTests
    {
        /// <summary>
        /// Tests whether the raising of the keyboard affects the position of canvas objects that have the component.
        /// Expects the object to move up when the keyboard is opened.
        /// </summary>
        [UnityTest]
        public IEnumerator Raising_OccursWhenKeyboardIsOpened_RectTransformIsMovedUp()
        {
            // Arrange.
            Canvas canvas = new GameObject().AddComponent<Canvas>();
            GameObject raised = new GameObject();
            raised.transform.SetParent(canvas.transform);
            RectTransform rectTransform = raised.AddComponent<RectTransform>();
            UIKeyboardRaiser raiser = raised.gameObject.AddComponent<UIKeyboardRaiser>();

            rectTransform.sizeDelta = new Vector2(100, 100);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.position = Vector3.zero;
            
            // Act.
            TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(string.Empty);

            yield return new WaitForSeconds(0.5f);
            
            // Assert.
            if (!TouchScreenKeyboard.visible)
                Assert.Pass();
            
            Assert.Greater(rectTransform.position.y, 0);

            // Clean up.
            Object.Destroy(canvas.gameObject);
            
            keyboard.active = false;
            yield return new WaitForSeconds(0.5f);
        }
        
        /// <summary>
        /// Tests whether the lowering of the keyboard affects the position of canvas objects that have the component.
        /// Expects the object to move back down when the keyboard is closed.
        /// </summary>
        [UnityTest]
        public IEnumerator Lowering_OccursWhenKeyboardIsClosed_RectTransformIsMovedDown()
        {
            // Arrange.
            Canvas canvas = new GameObject().AddComponent<Canvas>();
            GameObject raised = new GameObject();
            raised.transform.SetParent(canvas.transform);
            RectTransform rectTransform = raised.AddComponent<RectTransform>();
            UIKeyboardRaiser raiser = raised.gameObject.AddComponent<UIKeyboardRaiser>();

            rectTransform.sizeDelta = new Vector2(100, 100);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.position = Vector3.zero;

            // Act.
            TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(string.Empty);

            yield return new WaitForSeconds(0.5f);

            keyboard.active = false;
            
            yield return new WaitForSeconds(0.5f);
            
            // Assert.
            if (!TouchScreenKeyboard.visible)
                Assert.Pass();
            
            Assert.AreEqual(0, rectTransform.position.y);

            // Clean up.
            Object.Destroy(canvas.gameObject);
            
            keyboard.active = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}