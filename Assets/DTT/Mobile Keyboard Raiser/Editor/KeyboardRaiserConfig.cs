#if UNITY_EDITOR

using UnityEngine;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Object holding the debug settings for the editor keyboard.
    /// </summary>
    internal class KeyboardRaiserConfig : ScriptableObject
    {
        /// <summary>
        /// Whether arrows should be displayed showing the padding between the keyboard and the object.
        /// </summary>
        [SerializeField]
        private bool _enablePaddingArrow = true;

        /// <summary>
        /// Whether the area of the editor keyboard should be displayed.
        /// </summary>
        [SerializeField]
        private bool _enableTresholdArea = true;

        /// <summary>
        /// The amount of space the editor keyboard occupies.
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        private float _amountOccupied = .5f;

        /// <summary>
        /// Whether arrows should be displayed showing the padding between the keyboard and the object.
        /// </summary>
        public bool EnablePaddingArrow => _enablePaddingArrow;

        /// <summary>
        /// Whether the area of the editor keyboard should be displayed.
        /// </summary>
        public bool EnableTresholdArea => _enableTresholdArea;

        /// <summary>
        /// The amount of space the editor keyboard occupies.
        /// </summary>
        public float AmountOccupied => _amountOccupied;
    }
}

#endif