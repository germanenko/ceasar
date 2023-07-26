using DTT.Utils.EditorUtilities;
using UnityEditor;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Property cache for the <see cref="PaletteColorSelection"/>.
    /// </summary>
    internal class PaletteColorSelectionPropertyCache : RelativePropertyCache
    {
        /// <summary>
        /// The main property.
        /// </summary>
        public SerializedProperty Property { get; }
        
        /// <summary>
        /// The GUID for the palette.
        /// </summary>
        public SerializedProperty PaletteGuid => base["_paletteGuid"];
        
        /// <summary>
        /// The GUID for the color.
        /// </summary>
        public SerializedProperty PaletteColorGuid => base["_paletteColorGuid"];
        
        /// <summary>
        /// Initializes the property cache.
        /// </summary>
        /// <param name="serializedProperty">The property being cached.</param>
        public PaletteColorSelectionPropertyCache(SerializedProperty serializedProperty) : base(serializedProperty)
        {
            Property = serializedProperty;
        }
    }
}