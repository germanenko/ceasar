using System;
using System.Collections;
using DTT.KeyboardRaiser;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.KeyboardRaiser.Example
{
    /// <summary>
    /// Used for managing the UI for a sample that displays essential information the package manages.
    /// </summary>
    public class KeyboardStateExample : MonoBehaviour
    {
        /// <summary>
        /// The text object for displaying the keyboard height in pixels.
        /// </summary>
        [SerializeField]
        private Text _heightPixelsText;
        
        /// <summary>
        /// The text object for displaying the keyboard height in percentages.
        /// </summary>
        [SerializeField]
        private Text _heightPropText;
        
        /// <summary>
        /// The text object for displaying the keyboard state of being raised.
        /// </summary>
        [SerializeField]
        private Text _keyboardStateText;
        
        /// <summary>
        /// The image used for showing that the keyboard lowered event was triggered.
        /// </summary>
        [SerializeField]
        private Image _loweredImg;
        
        /// <summary>
        /// The image used for showing that the keyboard raised event was triggered.
        /// </summary>
        [SerializeField]
        private Image _raisedImg;

        /// <summary>
        /// Subscribes to lowered/raised events.
        /// </summary>
        private void OnEnable()
        {
            KeyboardStateManager.Current.Lowered += OnLowered;
            KeyboardStateManager.Current.Raised += OnRaised;
        }

        /// <summary>
        /// Unsubscribes to lowered/raised events.
        /// </summary>
        private void OnDisable()
        {
            KeyboardStateManager.Current.Lowered -= OnLowered;
            KeyboardStateManager.Current.Raised -= OnRaised;
        }

        /// <summary>
        /// Polls the <see cref="KeyboardStateManager"/> for the latest state and displays that.
        /// </summary>
        private void Update()
        {
            _heightPixelsText.text = $"Pixels: <color=\"#E65540\">{KeyboardStateManager.Current.PixelHeight}</color>";
            _heightPropText.text = $"%: <color=\"#E65540\">{KeyboardStateManager.Current.ProportionalHeight}</color>";
            _keyboardStateText.text = $"Is raised: <color=\"#E65540\">{KeyboardStateManager.Current.IsRaised}</color>";
        }

        /// <summary>
        /// Starts the image indication animation.
        /// </summary>
        private void OnRaised() => StartCoroutine(LightUp(_raisedImg));
        
        /// <summary>
        /// Starts the image indication animation.
        /// </summary>
        private void OnLowered() => StartCoroutine(LightUp(_loweredImg));

        /// <summary>
        /// Animation for lighting up an image for a short while before turning it back.
        /// </summary>
        /// <param name="img">The image to recolour.</param>
        private IEnumerator LightUp(Image img)
        {
            ColorUtility.TryParseHtmlString("#E65540", out Color color);
            img.color = color;
            yield return new WaitForSeconds(0.5f);
            ColorUtility.TryParseHtmlString("#D8D8D8", out color);
            color.a = .5f;
            img.color = color;
        }
    }
}