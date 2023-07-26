#if UNITY_2020_1_OR_NEWER

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Object = UnityEngine.Object;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Serializable implementation of the <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The serializable key value pair.
        /// </summary>
        [Serializable]
        internal struct SerializablePair
        {
            /// <summary>
            /// The serializable key.
            /// </summary>
            public TKey key;
            
            /// <summary>
            /// The serializable value.
            /// </summary>
            public TValue value;

            /// <summary>
            /// A pair is still a draft in the key value is still null reference. This can happen
            /// when classes are used as keys. Their default value is null so they can't be added
            /// to the dictionary yet.
            /// </summary>
            public bool IsDraft => key == null;
        }

        /// <summary>
        /// A list of the pairs to be serialized.
        /// </summary>
        [SerializeField]
        private List<SerializablePair> _pairs = new List<SerializablePair>();

        /// <summary>
        /// Whether the dictionary requires drafts.
        /// </summary>
        private readonly bool _requiresDraft = !typeof(TKey).IsValueType && typeof(TKey) != typeof(string);

        /// <summary>
        /// Refreshes the serializable pairs list with values in the dictionary.
        /// </summary>
        public void OnBeforeSerialize()
        {
            // Make sure only one draft persists.
            ReduceToOneDraft();

            // Add dictionary pairs to the serializable list.
            foreach (KeyValuePair<TKey, TValue> pair in this)
                _pairs.Add(new SerializablePair(){ key = pair.Key, value = pair.Value });
            
            // Rotate the list of pairs if we are using drafts, to move the draft to the back.
            if (_requiresDraft && _pairs.Count > 1)
                RotatePairsLeft(_pairs.Count(pair => !pair.IsDraft));
        }

        /// <summary>
        /// Refreshes the dictionary state with serialized keys and values.
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Clear();

            // Add serialized pairs back as dictionary entries.
            for (int i = 0; i < _pairs.Count; i++)
            {
                SerializablePair pair = _pairs[i];
                
                // Drafts are not added to the dictionary.
                if (pair.IsDraft)
                    continue;
                
                if (this.ContainsKey(pair.key))
                {
                    // If the key is already in the dictionary (because in the editor expanding the array/list will
                    // add an entry with the same value as the previous), try adding an entry with a default values.
                    SerializablePair instance = CreateDefaultInstance();
                    if (instance.IsDraft || !this.ContainsKey(instance.key))
                        _pairs[i] = instance;
                }

                try
                {
                    // Make sure the adding of duplicate values doesn't cause an exception.
                    this.Add(_pairs[i].key, _pairs[i].value);
                }
                catch (Exception)
                {
                    TKey invalidKey = pair.key;
                    
                    // If the added entry wasn't a draft, we give a warning.
                    if(!_pairs[i].IsDraft)
                        Debug.LogWarning($"Couldn't add {GetKeyInfo(invalidKey)} because it was already in the dictionary.");
                }
            }
        }

        /// <summary>
        /// Creates an instance of a serializable pair using default values.
        /// </summary>
        /// <returns>The created instance.</returns>
        private SerializablePair CreateDefaultInstance()
        {
            Type keyType = typeof(TKey);
            TKey key;
            if (keyType == typeof(string))
            {
                // Since the string class has no default constructor, it needs to be created using an argument.
                key = (TKey)Activator.CreateInstance(keyType, new object[] { Array.Empty<char>() });
            }
            else if (keyType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                // The default value for a unity object should be null in this case so it can be a draft.
                key = (TKey) (object) null;
            }
            else
            {
                key = Activator.CreateInstance<TKey>();
            }

            return new SerializablePair() { key = key, value = default };
        }

        /// <summary>
        /// Returns key a key info string used for logging. 
        /// </summary>
        /// <param name="key">The key of which to get the info string.</param>
        /// <returns>The info string.</returns>
        private string GetKeyInfo(TKey key)
        {
            if (Equals(key, null))
            {
                Type type = typeof(TKey);
                string prefix = type.IsSubclassOf(typeof(Object)) ? type.Name : "null";
                return prefix + " instance";
            }

            return key.ToString();
        }

        /// <summary>
        /// Reduces the amount of drafts in the pairs list to one.
        /// </summary>
        private void ReduceToOneDraft()
        {
            // Remove all non drafts first.
            _pairs.RemoveAll(pair => !pair.IsDraft);
            
            // Ensure the is only one draft left.
            if (_pairs.Count > 1)
            {
                SerializablePair first = _pairs[0];
                _pairs.Clear();
                _pairs.Add(first);
            }
        }
        
        /// <summary>
        /// Rotates the items in the pairs list to the left for the given amount of places.
        /// </summary>
        /// <param name="places">The amount of places to move the items.</param>
        private void RotatePairsLeft(int places)
        {
            SerializablePair pair;
            for (int i = 0; i < places; i++)
            {
                pair = _pairs[_pairs.Count - 1];
                _pairs.RemoveAt(_pairs.Count - 1);
                _pairs.Insert(0, pair);
            }
        }
    }
}

#endif