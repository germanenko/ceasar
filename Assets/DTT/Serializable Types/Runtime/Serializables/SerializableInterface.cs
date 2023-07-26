#if UNITY_2020_1_OR_NEWER

using System;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Represents an interface that can be serialized by wrapping a serialized unity object.
    /// </summary>
    /// <typeparam name="T">The type of interface value to use.</typeparam>
    [Serializable]
    public class SerializableInterface<T> where T : class
    {
        /// <summary>
        /// The serialized unity object value.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object _value;

        /// <summary>
        /// The interface value defined by T that is implemented by the unity object.
        /// </summary>
        public T Value
        {
            get
            {
                // Null check using normal null comparison.
                if (_value == null)
                    return null;
                
                // Check using equality without unity override on object to do unset check.
                T value = _value as T;
                if (Equals(value, null))
                    throw new InvalidCastException($"Could not cast {_value.GetType().Name} to {typeof(T).Name}.");

                return value;
            }
        }

        /// <summary>
        /// Whether a value has been assigned.
        /// </summary>
        public bool HasValue => _value != null;
    }
}

#endif