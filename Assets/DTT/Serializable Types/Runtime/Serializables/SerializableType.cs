using System;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Represents a serializable system type.
    /// </summary>
    [Serializable]
    public class SerializableType
    {
        /// <summary>
        /// The type value.
        /// </summary>
        public Type Value
        {
            get
            {
                if (_value != null)
                {
                    // Return the value if it is initialized.
                    return _value;
                }
 
                try
                {
                    // Try returning the assigned value using the static GetType method.
                    return _value = Type.GetType(_assemblyQualifiedName, true);
                }
                catch(Exception e)
                {
                    Debug.LogWarning(
                        $"The assembly qualified name {_assemblyQualifiedName} is invalid. Resetting it.\n" +
                        e.Message);
                    _assemblyQualifiedName = string.Empty;
                    return null;
                }
            }
            set
            {
                _value = value;
                _assemblyQualifiedName = value != null ? value.AssemblyQualifiedName : string.Empty;
            }
        }

        /// <summary>
        /// The assembly qualified name for this type.
        /// </summary>
        [SerializeField]
        private string _assemblyQualifiedName;

        /// <summary>
        /// The type value.
        /// </summary>
        private Type _value;

        /// <summary>
        /// Creates a new serializable type without a value.
        /// </summary>
        public SerializableType() => _assemblyQualifiedName = string.Empty;

        /// <summary>
        /// Creates a new serializable type based on given Type value.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        public SerializableType(Type type) => _assemblyQualifiedName = type != null ? type.AssemblyQualifiedName : string.Empty;

        /// <summary>
        /// Checks whether the object is the same as this object.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>Whether the object is the same as the other object.</returns>
        public override bool Equals(object obj)
        {
            SerializableType type = obj as SerializableType;
            if (type == null)
                return false;

            return this.Equals(type);
        }

        /// <summary>
        /// Checks whether the given type is the same as this one.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Whether the types are the same.</returns>
        public bool Equals(SerializableType type) => _assemblyQualifiedName.Equals(type._assemblyQualifiedName);

        /// <summary>
        /// Checks whether the given type is the same as this one.
        /// </summary>
        /// <param name="lhs">The first type to check.</param>
        /// <param name="rhs">The second type to check.</param>
        /// <returns>Whether the types are equal.</returns>
        public static bool operator ==(SerializableType lhs, SerializableType rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
                return false;

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// != Operator overload.
        /// Does the != in this fashion !(a==b).
        /// </summary>
        /// <param name="lhs">The first object to check.</param>
        /// <param name="rhs">The second object to check.</param>
        /// <returns>Whether the two objects are not equal.</returns>
        public static bool operator !=(SerializableType lhs, SerializableType rhs) => !(lhs == rhs);

        /// <summary>
        /// Converts a serialized type to a system type.
        /// </summary>
        /// <param name="type">The serialized type to convert.</param>
        public static implicit operator Type(SerializableType type) => type.Value;

        /// <summary>
        /// Converts a type to a serialized type.
        /// </summary>
        /// <param name="type">The system type.</param>
        public static implicit operator SerializableType(Type type) => new SerializableType(type);

        /// <summary>
        /// Gets a hash from the type.
        /// </summary>
        /// <returns>The hash of the current value.</returns>
        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => Value.AssemblyQualifiedName;
    }
}


