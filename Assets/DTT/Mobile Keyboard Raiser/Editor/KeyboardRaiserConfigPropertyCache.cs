#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using UnityEditor;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Property cache for the <see cref="KeyboardRaiserConfig"/>.
    /// </summary>
    internal class KeyboardRaiserConfigPropertyCache : SerializedPropertyCache
    {
        /// <summary>
        /// The serialized property for the <see cref="KeyboardRaiserConfig._enablePaddingArrow"/> toggle.
        /// </summary>
        public SerializedProperty enablePaddingArrow => base[nameof(enablePaddingArrow)];

        /// <summary>
        /// The serialized property for the <see cref="KeyboardRaiserConfig._enableTresholdArea"/> toggle.
        /// </summary>
        public SerializedProperty enableTresholdArea => base[nameof(enableTresholdArea)];

        /// <summary>
        /// The serialized property for the <see cref="KeyboardRaiserConfig._amountOccupied"/> slider.
        /// </summary>
        public SerializedProperty amountOccupied => base[nameof(amountOccupied)];

        /// <summary>
        /// Creates on object that contains all the serialized properties from <see cref="KeyboardRaiserConfig"/>.
        /// </summary>
        /// <param name="serializedObject">The object to gather the serialized properties from.</param>
        public KeyboardRaiserConfigPropertyCache(SerializedObject serializedObject) : base(serializedObject) { }
    }
}

#endif