using System.Linq;
using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Provides a custom drawer for <see cref="PaletteSelection"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(PaletteSelection))]
    internal class PaletteSelectionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Whether this is initialized.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// The dropdown containing all selectable options.
        /// </summary>
        private ExtendedDropdown _dropdown;

        /// <summary>
        /// The property cache for retrieving serialized properties.
        /// </summary>
        private PaletteColorSelectionPropertyCache _propertyCache;

        /// <summary>
        /// Texture cache containing loaded textures.
        /// </summary>
        private ColorPaletteManagementTextures _textureCache;

        /// <summary>
        /// Style cache containing reusable styling.
        /// </summary>
        private ColorPaletteManagementStyleCache _styleCache;

        /// <summary>
        /// Helps generating icons for the drawer.
        /// </summary>
        private SwatchGenerator _swatchGenerator;

        /// <summary>
        /// The target object.
        /// </summary>
        private PaletteSelection _target;

        /// <summary>
        /// Initializes the drawer.
        /// </summary>
        /// <param name="property">The property the drawer is for.</param>
        private void Initialize(SerializedProperty property)
        {
            // Mark initialized.
            _initialized = true;

            _propertyCache = new PaletteColorSelectionPropertyCache(property);
            _textureCache = new ColorPaletteManagementTextures();
            _styleCache = new ColorPaletteManagementStyleCache();
            _swatchGenerator = new SwatchGenerator();
            _target = (PaletteSelection) fieldInfo.GetValue(property.serializedObject.targetObject);
        }

        /// <summary>
        /// Draws the drawer.
        /// </summary>
        /// <param name="position">Where the drawer can be drawn.</param>
        /// <param name="property">The property of the drawer.</param>
        /// <param name="label">The label for the drawer.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Initialize if not done so.
            if (!_initialized)
                Initialize(property);

            Rect valueRect = EditorGUI.PrefixLabel(position, label);

            DrawPaletteButton(position, valueRect);

            DrawPreviewSwatch(valueRect);
            DrawSelectedName(valueRect);
        }

        /// <summary>
        /// Draws the name of the selector color and palette.
        /// </summary>
        /// <param name="valueRect">The rect to draw in.</param>
        private void DrawSelectedName(Rect valueRect)
        {
            Rect valueLabelRect = new Rect(valueRect)
            {
                position = new Vector2(valueRect.x + (_target .Palette == null || _target.Palette.Count == 0 ? 20 : 36), valueRect.y)
            };

            string name = _target.Palette == null ? "Missing reference" : $"<b>{_target.Palette.Name}</b>";

            EditorGUI.LabelField(valueLabelRect, name, new GUIStyle(GUI.skin.label) {richText = true});
        }

        /// <summary>
        /// Draws an icon of the selected color. 
        /// </summary>
        /// <param name="valueRect">The rect where this can be drawn.</param>
        private void DrawPreviewSwatch(Rect valueRect)
        {
            Rect swatchPosition = new Rect(valueRect)
            {
                position = new Vector2(valueRect.x + 22, valueRect.y + 3),
                size = new Vector2(13, 13)
            };

            if (_target.Palette != null && _target.Palette.Count > 0)
            {
                Color targetColor = _target.Palette.First().Color;
                GUI.DrawTexture(swatchPosition, _swatchGenerator.Generate(targetColor));
            }
        }

        /// <summary>
        /// Draws the button to open the selection window.
        /// </summary>
        /// <param name="position">Position to place the selection window.</param>
        /// <param name="valueRect">The rect to place the window.</param>
        private void DrawPaletteButton(Rect position, Rect valueRect)
        {
            Rect buttonRect = new Rect(valueRect)
            {
                size = new Vector2(18, 18),
                position = new Vector2(valueRect.x, valueRect.y)
            };

            GUIContent paletteContent = new GUIContent(_textureCache.Palette, "Allows you to select a different palette");
            if (GUI.Button(buttonRect, paletteContent, _styleCache.IconButton))
            {
                BuildExtendedDropdown(position);
                _dropdown.Show();
            }
        }

        /// <summary>
        /// Called when a palette color has been selected.
        /// </summary>
        /// <param name="palette">The palette that is selected.</param>
        private void OnPaletteColorSelected(Palette palette)
        {
            _target.Palette = palette;
            _propertyCache.PaletteGuid.stringValue = palette.GUID.ToString();
            _propertyCache.Property.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Builds the dropdown at the given position.
        /// </summary>
        /// <param name="position">Position to build the dropdown at.</param>
        private void BuildExtendedDropdown(Rect position)
        {
            ExtendedDropdownBuilder builder =
                new ExtendedDropdownBuilder("Palette Color Selector", position, new AdvancedDropdownState());

            foreach (var palette in PaletteDatabase.Instance.Palettes)
            {
                bool disabled = _target.Palette == palette;
                if (palette.Count > 0)
                {
                    Texture2D texture = _swatchGenerator.Generate(palette.First().Color);
                    builder.AddItem(palette.Name, disabled, texture, () => OnPaletteColorSelected(palette));
                }
                else
                {
                    builder.AddItem(palette.Name, disabled, () => OnPaletteColorSelected(palette));
                }
            }

            _dropdown = builder;
            _dropdown.AddMinimumSize(new Vector2(position.width / 2, 150));
        }
    }

}