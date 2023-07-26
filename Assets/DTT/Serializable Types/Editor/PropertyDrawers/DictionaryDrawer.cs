#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Forces the serializable dictionary class to draw the list of pairs
    /// instead of a foldout. *Note this will only work for Unity 2020 and up
    /// as property drawers for generic types are only supported from that version.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class DictionaryDrawer : PropertyDrawer
    {
        /// <summary>
        /// Returns the calculated height for the pairs list.
        /// </summary>
        /// <param name="property">The serializable dictionary property.</param>
        /// <param name="label">The label used for the dictionary.</param>
        /// <returns>The calculated height.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_pairs"), label, true);

        /// <summary>
        /// Draws the list of pairs in the inspector.
        /// </summary>
        /// <param name="position">The position at which to draw the dictionary.</param>
        /// <param name="property">The serializable dictionary property.</param>
        /// <param name="label">The label used for the dictionary.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_pairs"), label, true);
            EditorGUI.EndProperty();
        }
    }
}

#endif