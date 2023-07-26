#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Draws a serializable interface field in the inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    public class InterfacePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// The property name that should be drawn.
        /// </summary>
        public const string PROPERTY_NAME = "_value";

        /// <summary>
        /// Returns the height needed to draw the property.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        /// <returns>The height.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property.FindPropertyRelative(PROPERTY_NAME), label, true);

        /// <summary>
        /// Draws the serializable interface field in the inspector.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty value = property.FindPropertyRelative(PROPERTY_NAME);
            
            EditorGUI.BeginProperty(position, label, value);

            Object current = value.objectReferenceValue;
            Object newValue = EditorGUI.ObjectField(position, label, current, typeof(Object), true);
            if (current != newValue)
                TryAssignNewObjectReference(value, newValue);

            EditorGUI.EndProperty();
        }
        
        /// <summary>
        /// Tries assigning given object value to a serialized property.
        /// </summary>
        /// <param name="property">The property to assign the new object value to.</param>
        /// <param name="newValue">The new object value.</param>
        private void TryAssignNewObjectReference(SerializedProperty property, Object newValue)
        {
            // If the new value is null, we just set the object reference value to null.
            if (newValue == null)
            {
                property.objectReferenceValue = null;
                return;
            }

            Type constrainedType = GetTypeConstrained();

            // Use the component value if the dragged object is a game object. 
            if (newValue is GameObject gameObject)
                newValue = gameObject.GetComponent(constrainedType);

            // Assign the dragged object if it implements the interface.
            if (newValue != null && CanAssignObjectReference(newValue.GetType(), constrainedType))
                property.objectReferenceValue = newValue;
        }

        /// <summary>
        /// Returns whether a dragged type can be assigned based on a constrain type.
        /// </summary>
        /// <param name="draggedType">The dragged type to check.</param>
        /// <param name="constrainedType">The constrain type.</param>
        /// <returns>Whether the dragged type can be assigned.</returns>
        private bool CanAssignObjectReference(Type draggedType, Type constrainedType)
        {
            // Check assignability if the constrained type is an interface type.
            if (constrainedType.IsInterface)
                return constrainedType.IsAssignableFrom(draggedType);

            // If the constrained type is not an interface type we check for equality or inheritance.
            return draggedType == constrainedType || draggedType.IsSubclassOf(constrainedType);
        }

        /// <summary>
        /// Returns the type constrained used for the serialized interface property.
        /// </summary>
        /// <returns>The type constrained.</returns>
        private Type GetTypeConstrained()
        {
            Type fieldType = fieldInfo.FieldType;
            
            if (fieldType.BaseType == typeof(Array))
                fieldType = fieldType.GetElementType();
            else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                fieldType = fieldType.GetGenericArguments()[0];
            
            return fieldType.GetGenericArguments()[0];
        }
    }
}

#endif