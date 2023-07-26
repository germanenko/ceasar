using System;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor.OverviewEditor
{
    /// <summary>
    /// Used as a popup for renaming a value.
    /// </summary>
    internal class RenamePopup : PopupWindowContent
    {
        /// <summary>
        /// Called when the user confirmed their action.
        /// </summary>
        public event Action<string> Renamed;
        
        /// <summary>
        /// The current value of the rename.
        /// </summary>
        private string _value;

        /// <summary>
        /// Creates a new rename popup.
        /// </summary>
        /// <param name="value">The initial value of the rename field.</param>
        public RenamePopup(string value) => _value = value;

        /// <summary>
        /// Draws the field.
        /// </summary>
        /// <param name="rect">The rect to draw it in.</param>
        public override void OnGUI(Rect rect)
        {
            editorWindow.maxSize = new Vector2(editorWindow.maxSize.x, 44);
            _value = EditorGUILayout.TextField(_value);
            if (GUILayout.Button("Confirm"))
            {
                Renamed?.Invoke(_value);
                editorWindow.Close();
            }
        }
    }
}