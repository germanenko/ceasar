#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Draws the serializable type in the inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        /// <summary>
        /// The internal serialized type internals for setting it when the callback is called.
        /// </summary>
        private SerializedProperty _serializedType;

        /// <summary>
        /// Whether or not a lookup for the filter attribute has been done.
        /// </summary>
        private bool _didAttributeLookup;

        /// <summary>
        /// The type used for filtering.
        /// </summary>
        private Type _filterType;

        /// <summary>
        /// The base height of the property.
        /// </summary>
        /// <param name="property">The property to draw.</param>
        /// <param name="label">The label to draw.</param>
        /// <returns>The height of the property to draw.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

        /// <summary>
        /// The main draw call of the property.
        /// </summary>
        /// <param name="rect">The rectangle  to draw the element in.</param>
        /// <param name="property">The property of the serialized field.</param>
        /// <param name="label">The label of the serialized field.</param>
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            _serializedType = property;

            EditorGUI.BeginProperty(rect, label, property);

            DrawPrefix(rect, label);
            DrawDropdownButton(rect);

            EditorGUI.EndProperty();
        }
        
        /// <summary>
        /// Draws the prefix in front of the dropdown button.
        /// </summary>
        /// <param name="rect">The rectangle in which the property is drawn.</param>
        /// <param name="label">The label of the property.</param>
        private void DrawPrefix(Rect rect, GUIContent label)
        {
            Rect labelRect = new Rect(rect.position, new Vector2(EditorGUIUtility.labelWidth, rect.height));
            EditorGUI.LabelField(labelRect, label);
        }

        /// <summary>
        /// Draws the dropdown button that when clicked opens a popup window.
        /// </summary>
        /// <param name="rect">The rectangle in which the property is drawn.</param>
        private void DrawDropdownButton(Rect rect)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            GUIContent label = GetLabelBasedOnCurrentType();

            Vector2 size = new Vector2(rect.width - labelWidth, rect.height);
            Rect ButtonRect = new Rect(rect.position + new Vector2(labelWidth, 0), size);

            DrawDropdownButton(ButtonRect, label);
        }

        /// <summary>
        /// Returns a label based on the currently serialized type its assembly qualified name value.
        /// </summary>
        /// <returns>The label.</returns>
        private GUIContent GetLabelBasedOnCurrentType()
        {
            string assemblyQualifiedName = _serializedType.FindPropertyRelative("_assemblyQualifiedName").stringValue;
            if (string.IsNullOrEmpty(assemblyQualifiedName))
                return new GUIContent(string.Empty);

            try
            {
                Type type = Type.GetType(assemblyQualifiedName);
                return new GUIContent(type.Name);
            }
            catch
            {
                string[] split = assemblyQualifiedName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string typeName = split.Length != 0 ? split[0] : "Unidentified";
                return new GUIContent($"Missing ({typeName})");
            }
        }

        /// <summary>
        /// Draws a dropdown button that when clicked shows a popup window with type selector content.
        /// </summary>
        /// <param name="rect">The rectangle to draw the button in.</param>
        /// <param name="label">The button label to use.</param>
        private void DrawDropdownButton(Rect rect, GUIContent label)
        {
            if (!EditorGUI.DropdownButton(rect, label, FocusType.Passive))
                return;

            if (!_didAttributeLookup)
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(TypeFilterAttribute), true);
                if (attributes.Length != 0)
                    _filterType = ((TypeFilterAttribute)attributes[0]).filterType;
                else
                    _filterType = typeof(object);

                _didAttributeLookup = true;
            }


            PopupWindow.Show(rect, new TypeSelectorContent(_filterType, rect.width, OnTypeSelected));
        }

        /// <summary>
        /// Called when a type has been selected to assign the new assembly qualified name.
        /// </summary>
        /// <param name="assemblyQualifiedName">The assemblyQualifiedName that is returned.</param>
        private void OnTypeSelected(string assemblyQualifiedName)
        {
            _serializedType.FindPropertyRelative("_assemblyQualifiedName").stringValue = assemblyQualifiedName;
            _serializedType.serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif