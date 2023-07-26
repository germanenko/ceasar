#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Overrides the standard inspector of the <see cref="KeyboardRaiserConfig"/> and removes the standards property fields.
    /// </summary>
    [CustomEditor(typeof(KeyboardRaiserConfig))]
    [DTTHeader("dtt.keyboardraiser", "Keyboard Raiser Config")]
    internal class KeyboardRaiserConfigEditor : DTTInspector
    {
        /// <summary>
        /// Overrides the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("This object holds the debug settings of the keyboard raiser.");
        }
    }
}

#endif