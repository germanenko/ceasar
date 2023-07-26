using DTT.COLOR.Editor.OverviewEditor;
using DTT.PublishingTools;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Custom editor for the Palette Database Scriptable Object to enforce editing in the window.
    /// </summary>
    [CustomEditor(typeof(PaletteDatabase))]
    [DTTHeader("dtt.color-palette-manager", "Palette Database")]
    public class PaletteDatabaseEditor : DTTInspector
    {
        /// <summary>
        /// Draws the DTT Header and the button to open the window.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open window")) 
                ColorPaletteWindow.ShowWindow();
        }
    }
}