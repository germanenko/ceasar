#if UNITY_2020_1_OR_NEWER && TEST_FRAMEWORK

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTT.Utils.EditorUtilities.Serializables.Tests
{
    /// <summary>
    /// Tests the <see cref="SerializableDictionary{TKey,TValue}"/> class.
    /// </summary>
    public class Test_SerializableDictionary
    {
        /// <summary>
        /// The binding flags used for reflection during the test.
        /// </summary>
        private BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Tests whether the dictionary properly adds key value pairs to serializable lists
        /// before serialization. It expects the serialized key and value lists to hold the
        /// added key-value pairs before serialization starts.
        /// </summary>
        [Test]
        public void Test_OnBeforeSerialize()
        {
            // Arrange.
            int key = 5;
            float value = 15f;
            SerializableDictionary<int, float> dictionary = new SerializableDictionary<int, float>();
            dictionary.Add(key, value);

            // Act.
            dictionary.OnBeforeSerialize();
            var pairs = (List<SerializableDictionary<int,float>.SerializablePair>)dictionary
                .GetType()
                .GetField("_pairs", flags)
                .GetValue(dictionary);
            
            // Assert.
            Assert.IsNotEmpty(pairs, "Expected the serialized pairs to contain an entry but it didn't.");
            Assert.IsTrue(key == pairs[0].key, "Expected the key to be serialized in the list but it wasn't.");
            Assert.IsTrue(value == pairs[0].value, "Expected the key to be serialized in the list but it wasn't.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableDictionary{TKey, TValue}"/> can handle
        /// serializable key or value types. It expects no exception to bo be thrown when
        /// the key count is equal to the value count.
        /// </summary>
        [Test]
        public void Test_OnAfterDeserialize_SerializableKeyOrValue()
        {
            // Arrange.
            SerializableDictionary<int, float> dictionary = new SerializableDictionary<int, float>();
            FieldInfo pairsField = dictionary.GetType().GetField("_pairs", flags);
            
            // Act.
            List<SerializableDictionary<int,float>.SerializablePair> pairs = 
                (List<SerializableDictionary<int,float>.SerializablePair>)pairsField.GetValue(dictionary);
            
            IEnumerable<int> range = Enumerable.Range(1, 5);
            pairs.AddRange(range.Select(num => new SerializableDictionary<int,float>.SerializablePair()
            {
                key = num,
                value = 0.0f
            }));

            TestDelegate action = () => dictionary.OnAfterDeserialize();

            // Assert.
            Assert.DoesNotThrow(action, "Expected no exceptions to be thrown but there where.");
        }

        /// <summary>
        /// Tests whether the <see cref="SerializableDictionary{TKey, TValue}"/> can handle
        /// duplicate keys when the duplicate is a default value. It expects no exceptions
        /// to be thrown and no entry to be added.
        /// </summary>
        [Test]
        public void Test_OnAfterDeserialize_SerializableKeyOrValue_DuplicateKeys_DefaultValue()
        {
            // Arrange.
            SerializableDictionary<int, float> dictionary = new SerializableDictionary<int, float>();

            // Act.
            var pairs = (List<SerializableDictionary<int,float>.SerializablePair>)dictionary
                .GetType()
                .GetField("_pairs", flags)
                .GetValue(dictionary);

            IEnumerable<int> range = Enumerable.Range(1, 5);
             pairs.AddRange(range.Select(num => new SerializableDictionary<int,float>.SerializablePair()
            {
                key = 0,
                value = 0.0f
            }));

             TestDelegate action = () => dictionary.OnAfterDeserialize();
             
             // Assert.
             Assert.DoesNotThrow(action, "Expected no exceptions but there where.");
        }
        
        /// <summary>
        /// Tests whether the <see cref="SerializableDictionary{TKey, TValue}"/> can handle
        /// duplicate keys when the duplicate is a default value. It expects no exceptions
        /// to be thrown and no entry to be added.
        /// </summary>
        [Test]
        public void Test_OnAfterDeserialize_SerializableKeyOrValue_DuplicateKeys_DefaultValue_String()
        {
            // Arrange.
            SerializableDictionary<string, float> dictionary = new SerializableDictionary<string, float>();

            // Act.
            var pairs = (List<SerializableDictionary<string,float>.SerializablePair>)dictionary
                .GetType()
                .GetField("_pairs", flags)
                .GetValue(dictionary);
            
            pairs.Add( new SerializableDictionary<string,float>.SerializablePair()
            {
                key = "duplicate",
                value = 0.0f
            });
            
            pairs.Add( new SerializableDictionary<string,float>.SerializablePair()
            {
                key = "duplicate",
                value = 0.0f
            });


            TestDelegate action = () => dictionary.OnAfterDeserialize();
             
            // Assert.
            Assert.DoesNotThrow(action, "Expected no exceptions but there where.");
        }

        // <summary>
        /// Tests whether the <see cref="SerializableDictionary{TKey, TValue}"/> can handle
        /// duplicate keys when the duplicate is not a default value. It expects no exceptions
        /// to be thrown and an entry to be added with a default value.
        /// </summary>
        [Test]
        public void Test_OnAfterDeserialize_SerializableKeyOrValue_DuplicateKeys_NoDefaultValue()
        {
            // Arrange.
            SerializableDictionary<int, float> dictionary = new SerializableDictionary<int, float>();

            // Act.
            var pairs = (List<SerializableDictionary<int,float>.SerializablePair>)dictionary
                .GetType()
                .GetField("_pairs", flags)
                .GetValue(dictionary);

            pairs.Add(new SerializableDictionary<int, float>.SerializablePair(){key = 5, value = 0.0f });
            pairs.Add(new SerializableDictionary<int, float>.SerializablePair(){key = 5, value = 0.0f });

            TestDelegate action = () => dictionary.OnAfterDeserialize();

            // Assert.
            Assert.DoesNotThrow(action, "Expected no exceptions to be thrown but there where.");
            Assert.AreEqual(pairs.Count, dictionary.Count, "Expected the key to be added but it wasn't.");
        }

    }
}

#endif