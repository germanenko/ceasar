#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Draws the inspector for a serializable dictionary its key value pair.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>.SerializablePair))]
    public class DictionaryPairDrawer : PropertyDrawer
    {
        /// <summary>
        /// The horizontal spacing to use for drawing the dictionary.
        /// </summary>
        private const float HORIZONTAL_SPACING = 10f;
        
        /// <summary>
        /// Returns the calculated height for the property.
        /// </summary>
        /// <param name="property">The serializable pair property.</param>
        /// <param name="label">The label used for the property.</param>
        /// <returns>The calculated height.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float keyHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("key"), GUIContent.none, true);
            float valueHeight =
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative("value"), GUIContent.none, true);
            return Mathf.Max(keyHeight, valueHeight);
        }

        /// <summary>
        /// Draws the pair horizontally in the inspector.
        /// </summary>
        /// <param name="position">The position at which to draw the dictionary.</param>
        /// <param name="property">The serializable dictionary property.</param>
        /// <param name="label">The label used for the dictionary.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            Rect keyPosition = new Rect(position.x, position.y, labelWidth, position.height);
            EditorGUI.PropertyField(keyPosition, property.FindPropertyRelative("key"),GUIContent.none, true);

            float valuePositionX = position.x + labelWidth + HORIZONTAL_SPACING;
            float valuePositionWidth = position.width - labelWidth - HORIZONTAL_SPACING;
            Rect valuePosition = new Rect(valuePositionX, position.y, valuePositionWidth, position.height);
            EditorGUI.PropertyField(valuePosition, property.FindPropertyRelative("value"), GUIContent.none, true);
        }
    }
}

#endif