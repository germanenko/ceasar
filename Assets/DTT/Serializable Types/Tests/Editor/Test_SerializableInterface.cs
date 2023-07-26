#if UNITY_2020_1_OR_NEWER && TEST_FRAMEWORK

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace DTT.Utils.EditorUtilities.Serializables.Tests
{
    /// <summary>
    /// Tests the <see cref="SerializableInterface{T}"/> class.
    /// </summary>
    public class Test_SerializableInterface 
    {
        /// <summary>
        /// Tests the serializable usage of the class.
        /// </summary>
        [UnityTest]
        public IEnumerator Test_Serializable_Usage()
        {
            // Open the test editor window containing the ellipsis method tests. 
            SerializableInterfaceWindow window = EditorWindow.GetWindow<SerializableInterfaceWindow>("TestWindow", true);
            
            // Wait a frame before closing it.
            yield return null;

            // Close the window now that the test is done.
            window.Close();
        }
    }
}

#endif