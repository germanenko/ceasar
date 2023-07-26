#if UNITY_2020_1_OR_NEWER && TEST_FRAMEWORK

using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables.Tests
{
    /// <summary>
    /// Tests the serializable interface in a serializable environment.
    /// </summary>
    public class SerializableInterfaceWindow : EditorWindow
    {
        /// <summary>
        /// The interface used for testing.
        /// </summary>
        public interface ITestable { }

        /// <summary>
        /// The testable class implementing the interface.
        /// </summary>
        public class TestableClass : ScriptableObject, ITestable { }
        
        /// <summary>
        /// The testable class not implementing the interface.
        /// </summary>
        public class OtherTestableClass : ScriptableObject { }

        /// <summary>
        /// Whether the OnGUI method has been run.
        /// </summary>
        private bool _didCallOnGUI = false;
        
        /// <summary>
        /// The serializable interface instance to be tested.
        /// </summary>
        [SerializeField]
        private SerializableInterface<ITestable> _testInterface;
        
        /// <summary>
        /// Runs the tests.
        /// </summary>
        private void OnGUI()
        {
            if (_didCallOnGUI)
            {
                // Serializable interface.
                Test_SerializableInterface_Compatible_Value();
                Test_SerializableInterface_Incompatible_Value();
                Test_SerializableInterface_Null_Value();
                Test_SerializableInterface_HasValue_Compatible_Value();
                
                _didCallOnGUI = true;
            }
        }

        /// <summary>
        /// Tests whether a compatible value can be used after serialization.
        /// It expects the value to be valid after it has been assigned though serialization.
        /// </summary>
        private void Test_SerializableInterface_Compatible_Value()
        {
            // Arrange.
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_testInterface");
            
            // Act.
            SerializedProperty value = property.FindPropertyRelative("_value");
            value.objectReferenceValue = CreateInstance<TestableClass>();
            serializedObject.ApplyModifiedProperties();
            
            // Assert.
            Assert.NotNull(_testInterface.Value, "Expected the test interface value to be set but it wasn't.");
        }
       
        /// <summary>
        /// Tests whether a compatible value can be used after serialization.
        /// It expects the 'HasValue' property to return true if the value has been properly set.
        /// </summary>
        private void Test_SerializableInterface_HasValue_Compatible_Value()
        {
            // Arrange.
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_testInterface");
            
            // Act.
            SerializedProperty value = property.FindPropertyRelative("_value");
            value.objectReferenceValue = CreateInstance<TestableClass>();
            serializedObject.ApplyModifiedProperties();
            
            // Assert.
            Assert.IsTrue(_testInterface.HasValue, "Expected the test interface value to be set but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether a compatible value can be used after serialization.
        /// It expects the value not to be set if the serialized value is null.
        /// </summary>
        private void Test_SerializableInterface_Null_Value()
        {
            // Arrange.
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_testInterface");
            
            // Act.
            SerializedProperty value = property.FindPropertyRelative("_value");
            value.objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
            
            // Assert.
            Assert.Null(_testInterface.Value, "Expected the test interface value to be null but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether a compatible value can be used after serialization.
        /// It expects an incompatible value to cause a cast exception.
        /// </summary>
        private void Test_SerializableInterface_Incompatible_Value()
        {
            // Arrange.
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_testInterface");
            
            // Act.
            SerializedProperty value = property.FindPropertyRelative("_value");
            value.objectReferenceValue = CreateInstance<OtherTestableClass>();
            serializedObject.ApplyModifiedProperties();

            TestDelegate action = () =>
            {
                ITestable testValue = _testInterface.Value;
            };
            
            // Assert.
            Assert.Catch<InvalidCastException>(action, "Expected the test incompatible value to cause an exception but it didn't.");
        }
    }
}
#endif
