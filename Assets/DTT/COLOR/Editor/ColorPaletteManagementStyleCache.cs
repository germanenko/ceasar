using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Provides styling for the COLOR editors.
    /// </summary>
    internal class ColorPaletteManagementStyleCache : GUIStyleCache
    {
        /// <summary>
        /// For buttons that contains icons.
        /// </summary>
        public GUIStyle IconButton => base[nameof(IconButton)];
        
        /// <summary>
        /// Style used for scrolling views.
        /// </summary>
        public GUIStyle ScrollView => base[nameof(ScrollView)];
        
        /// <summary>
        /// A label with high font weight.
        /// </summary>
        public GUIStyle BoldLabel => base[nameof(BoldLabel)];
        
        /// <summary>
        /// A label with italics.
        /// </summary>
        public GUIStyle ItalicLabel => base[nameof(ItalicLabel)];

        /// <summary>
        /// Creates a new instance of ColorPaletteManagementStyleCache.
        /// </summary>
        public ColorPaletteManagementStyleCache()
        {
            Add(nameof(IconButton), () => new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(0, 0, 0, 0), 
                padding = new RectOffset(3, 3, 3, 3)
            });
            
            Add(nameof(ScrollView), () => new GUIStyle { margin = new RectOffset(9, 9, 9, 0) });
            Add(nameof(BoldLabel), () => EditorStyles.boldLabel);
            Add(nameof(ItalicLabel), () => new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });
        }
    }
}