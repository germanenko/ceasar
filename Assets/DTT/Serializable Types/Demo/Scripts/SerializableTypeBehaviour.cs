using System.Collections.Generic;
using UnityEngine;
using DTT.Utils.EditorUtilities.Serializables;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Holds multiple serializable type fields normally not supported by Unity's serialization system.
    /// </summary>
    internal class SerializableTypeBehaviour : MonoBehaviour
    {
#if UNITY_2020_1_OR_NEWER
        /// <summary>
        /// A serializable dictionary field, key/value pairs can be assigned in the inspector.
        /// </summary>
        [SerializeField]
        private SerializableDictionary<string, int> _stringIntDictionary;

        /// <summary>
        /// A serializable dictionary field, key/value pairs can be assigned in the inspector.
        /// </summary>
        [SerializeField]
        private SerializableDictionary<CharacterSettingsSO, float> _scriptableObjectDictionary;

        /// <summary>
        /// A serializable interface field. Only objects that implement the IEnumerable interface
        /// can be assigned in to this array in the inspector.
        /// </summary>
        [SerializeField]
        private List<SerializableInterface<IEnumerable<int>>> _enumerables;
#endif
        /// <summary>
        /// A serializable type field. A dropdown in the inspector can be used to assign a value to it.
        /// Using the TypeFilter attribute, only objects that derive from UnityEngine.Object will be shown.
        /// </summary>
        [SerializeField, TypeFilter(typeof(Object))]
        private SerializableType _type;
    }
}