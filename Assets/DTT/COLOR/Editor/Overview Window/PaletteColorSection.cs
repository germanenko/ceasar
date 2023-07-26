using System;
using DTT.Utils.Extensions;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor.OverviewEditor
{
    /// <summary>
    /// Used for drawing a color in the window.
    /// </summary>
    internal class PaletteColorSection : IDrawable
    {
        /// <summary>
        /// Whether the interaction button is pressed.
        /// </summary>
        public event Action ButtonPressed;

        /// <summary>
        /// Called when the up button is pressed.
        /// </summary>
        public event Action UpButtonPressed;
        
        /// <summary>
        /// Called when the down button is pressed.
        /// </summary>
        public event Action DownButtonPressed;
        
        /// <summary>
        /// The color backing this UI.
        /// </summary>
        public PaletteColor Color { get; }
        
        /// <summary>
        /// The textures used in this window.
        /// </summary>
        protected readonly ColorPaletteManagementTextures _textures;

        /// <summary>
        /// The styles used throughout the package.
        /// </summary>
        private readonly ColorPaletteManagementStyleCache _styleCache;

        /// <summary>
        /// Creates a new BasePaletteColorSection.
        /// </summary>
        /// <param name="color">The color backing this UI.</param>
        /// <param name="textures">The textures used in this window.</param>
        /// <param name="styleCache">The styles used in the package.</param>
        public PaletteColorSection(PaletteColor color, ColorPaletteManagementTextures textures, ColorPaletteManagementStyleCache styleCache)
        {
            Color = color;
            _textures = textures;
            _styleCache = styleCache;
        }
        
        /// <summary>
        /// Draws the field.
        /// </summary>
        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            {
                DrawColorField();
                DrawLabelField();
                DrawHexField();
                GUILayout.FlexibleSpace();
                DrawSwapButtons();
                GUILayout.Space(5);
                if(GUILayout.Button(_textures.TrashBin, _styleCache.IconButton, GUILayout.Width(18), GUILayout.Height(18)))
                    ButtonPressed?.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the color field.
        /// </summary>
        private void DrawColorField() => Color.Color = EditorGUILayout.ColorField(Color.Color, GUILayout.Width(53));

        /// <summary>
        /// Draws the name of the color.
        /// </summary>
        private void DrawLabelField() => Color.Name = EditorGUILayout.TextField(Color.Name, GUILayout.Width(Screen.width * 0.4f));

        /// <summary>
        /// Draws the hex value of this color.
        /// </summary>
        private void DrawHexField()
        {
            GUIStyle label = new GUIStyle(GUI.skin.label) { richText = true };
            EditorGUILayout.SelectableLabel(GetHex(), label, GUILayout.Height(18));
        }

        /// <summary>
        /// Draws the swap buttons and sets up the button behaviour.
        /// </summary>
        private void DrawSwapButtons()
        {
            if (GUILayout.Button(_textures.ArrowUp, _styleCache.IconButton, GUILayout.Width(18), GUILayout.Height(18)))
                UpButtonPressed?.Invoke();
            if (GUILayout.Button(_textures.ArrowDown, _styleCache.IconButton, GUILayout.Width(18), GUILayout.Height(18)))
                DownButtonPressed?.Invoke();
        }

        /// <summary>
        /// Returns a hex string with colored parts used in rich text environment.
        /// </summary>
        /// <returns>A hex string with colored parts used in rich text environment.</returns>
        private string GetHex()
        {
            string colorHex = Color.Color.a < 1
                ? ColorUtility.ToHtmlStringRGBA(Color.Color)
                : ColorUtility.ToHtmlStringRGB(Color.Color);

            string red = colorHex.Substring(0, 2);
            string green= colorHex.Substring(2, 2);
            string blue = colorHex.Substring(4, 2);
            string alpha = colorHex.Length == 8 ? colorHex.Substring(6, 2) : string.Empty;

            red = red.Color("EEB0B0");
            green = green.Color("BDEFB1");
            blue = blue.Color("B4B1EF");

            colorHex = red + green + blue + alpha;

            return "#".Bold() + colorHex;
        }
    }
}