using DTT.COLOR.ColorMatching;
using DTT.PublishingTools;
using UnityEditor;

namespace DTT.COLOR.Editor.ColorMatching
{
    /// <summary>
    /// Provides the editor for the <see cref="CentralColorPalette"/> component.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CentralColorPalette))]
    [DTTHeader("dtt.color-palette-manager", "Central Color Palette")]
    public class CentralColorPaletteEditor : DTTInspector
    {
        /// <summary>
        /// Draws the custom DTT inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
        }
    }
}