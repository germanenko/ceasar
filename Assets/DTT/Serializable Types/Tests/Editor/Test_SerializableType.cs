#if TEST_FRAMEWORK

using NUnit.Framework;
using System;

namespace DTT.Utils.EditorUtilities.Serializables.Tests
{
    /// <summary>
    /// Tests the <see cref="SerializableType"/> class.
    /// </summary>
    public class Test_SerializableType
    {
        #region Tests
        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can correctly construct 
        /// itself. It expects that when it is constructed with an type that is null
        /// the value will also be null.
        /// </summary>
        [Test]
        public void Test_Constructor_Invalid_Type()
        {
            // Arrange.
            Type type = null;

            // Act.
            SerializableType serializableType = new SerializableType(type);

            // Assert.
            Assert.IsNull(serializableType.Value, "Expected the value to be null after its initialization with a null type but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> returns a value representing
        /// its initialization state. It expects that if no type was given, its value is null.
        /// </summary>
        [Test]
        public void Test_Getter_Invalid_AssemblyQualifiedName()
        {
            SerializableType serializableType = new SerializableType();
            Type systemType = serializableType.Value;

            Assert.IsNull(systemType, "Expected the value to be null but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> returns a value representing
        /// its initialization state. It expects that if a type was given its value will not be null.
        /// </summary>
        [Test]
        public void Test_Getter_Valid_AssemblyQualifiedName()
        {
            Type expected = typeof(float);
            SerializableType serializableType = new SerializableType(expected);
            Type systemType = serializableType.Value;
            Type systemTypeTwo = serializableType.Value;

            Assert.AreEqual(expected.AssemblyQualifiedName, systemType.AssemblyQualifiedName,
                "Expected the assembly qualified name to be the same of both types but it wasn't.");

            Assert.IsNotNull(systemTypeTwo, "Expected the assigned value not to be null but it was.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> its value can be properly set.
        /// It expects no exceptions to be thrown when the value is set.
        /// </summary>
        [Test]
        public void Test_Setter()
        {
            TestDelegate action = () =>
            {
                SerializableType type = new SerializableType();
                type.Value = typeof(float);
            };

            Assert.DoesNotThrow(action, "Expected no exceptions to be thrown but there where.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly be compared.
        /// It expects the equality of two of the same types to return true.
        /// </summary>
        [Test]
        public void Test_Equality_Correct_Value()
        {
            SerializableType typeOne = new SerializableType(typeof(float));
            SerializableType typeTwo = new SerializableType(typeof(float));

            Assert.IsTrue(typeOne == typeTwo, "Expected the serializable types to be equal but they wheren't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly be compared.
        /// It expects the equality of two of the same types to return true.
        /// </summary>
        [Test]
        public void Test_Equality_Correct_Object_Value()
        {
            SerializableType typeOne = new SerializableType(typeof(float));
            SerializableType typeTwo = new SerializableType(typeof(float));

            Assert.IsTrue(typeOne.Equals((object)typeTwo), "Expected the serializable types to be equal but they wheren't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly be compared.
        /// It expects the equality of two of the same types to return true.
        /// </summary>
        [Test]
        public void Test_Equality_Correct_BothNull()
        {
            SerializableType typeOne = null;
            SerializableType typeTwo = null;

            Assert.IsTrue(typeOne == typeTwo, "Expected the serializable types to be equal but they wheren't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly be compared.
        /// It expects the equality of two different types to return false.
        /// </summary>
        [Test]
        public void Test_Equality_Incorrect()
        {
            SerializableType typeOne = new SerializableType(typeof(float));
            SerializableType typeTwo = new SerializableType(typeof(int));

            Assert.IsFalse(typeOne == typeTwo, "Expected the serializable types not to be equal but they where.");
            Assert.IsFalse(typeOne == null, "Expected the serializable type not to be equal to null but it was.");

            Assert.IsTrue(typeOne != typeTwo, "Expected the serializable types not to be equal but they where.");
        }


        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly be compared.
        /// It expects the equality of two different types to return false.
        /// </summary>
        [Test]
        public void Test_Equality_InCorrect_Object_Value_Null()
        {
            SerializableType typeOne = new SerializableType(typeof(float));
            object objectValue = null;

            Assert.IsFalse(typeOne.Equals(objectValue), "Expected the objects not to be equal but they where.");
        }


        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly converted
        /// to a type. It expects an implicit conversion to end up with both having the same assembly qualified name. 
        /// </summary>
        [Test]
        public void Test_Implicit_Conversion_SerializableType_To_Type()
        {
            Type expected = typeof(float);
            SerializableType serializableType = new SerializableType(expected);
            Type systemType = serializableType;

            Assert.AreEqual(expected.AssemblyQualifiedName, systemType.AssemblyQualifiedName,
                "Expected the assembly qualified name to be the same of both types but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly converted
        /// from a type. It expects an implicit conversion to end up with both having the same assembly qualified name. 
        /// </summary>
        [Test]
        public void Test_Implicit_Conversion_Type_To_SerializableType()
        {
            Type expected = typeof(float);
            SerializableType serializableType = expected;

            Assert.AreEqual(expected.AssemblyQualifiedName, serializableType.Value.AssemblyQualifiedName,
                "Expected the assembly qualified name to be the same of both types but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly converted
        /// to a string. It expects the string value to represent the assembly qualified name.
        /// </summary>
        [Test]
        public void Test_ToString()
        {
            SerializableType type = new SerializableType(typeof(float));

            Assert.AreEqual(typeof(float).AssemblyQualifiedName, type.ToString(),
                "Expected the ToString method to return the assembly qualified type but it didn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly converted
        /// to a hash. It expects the hash to be null when it hasn't been initialized.
        /// </summary>
        [Test]
        public void Test_GetHashCode_Null()
        {
            SerializableType type = new SerializableType();
            Assert.AreEqual(0, type.GetHashCode(), "Expected the hash code to be 0 but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableType"/> can be correctly converted
        /// to a hash. It expects the hash to be the same as the type hash.
        /// </summary>
        [Test]
        public void Test_GetHashCode_NotNull()
        {
            SerializableType type = new SerializableType(typeof(float));
            Assert.AreEqual(typeof(float).GetHashCode(), type.GetHashCode(), "Expected the hash codes to be equal but they wheren't.");
        }
        #endregion
    }
}

#endif
