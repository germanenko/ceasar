using DTT.Utils.EditorUtilities;
using UnityEditor;

namespace DTT.COLOR.Editor.ColorMatching
{
    /// <summary>
    /// Property cache for <see cref="OrdinalColorMatcher"/>.
    /// </summary>
    internal class OrdinalColorMatcherPropertyCache : SerializedPropertyCache
    {
        /// <summary>
        /// Serialized property for _centralColorPalette.
        /// </summary>
        public SerializedProperty CentralColorPalette => base["_centralColorPalette"];
        
        /// <summary>
        /// Serialized property for _ordinal.
        /// </summary>
        public SerializedProperty Ordinal => base["_ordinal"];
        
        /// <summary>
        /// Serialized property for _overrideAlpha.
        /// </summary>
        public SerializedProperty OverrideAlpha => base["_overrideAlpha"];
        
        /// <summary>
        /// Creates a new property cache instance.
        /// </summary>
        public OrdinalColorMatcherPropertyCache(SerializedObject serializedObject) : base(serializedObject)
        {
        } 
    }
}