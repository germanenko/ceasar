using System;
using System.Linq;
using System.Text;
using DTT.COLOR.ColorMatching;
using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using DTT.Utils.Extensions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DTT.COLOR.Editor.ColorMatching
{
    /// <summary>
    /// Provides an editor for <see cref="OrdinalColorMatcher"/>.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(OrdinalColorMatcher))]
    [DTTHeader("dtt.color-palette-manager", "Ordinal Color Matcher")]
    internal class OrdinalColorMatcherEditor : DTTInspector
    {
        /// <summary>
        /// The dropdown for displaying the ordinal colors.
        /// </summary>
        private ExtendedDropdown _dropdown;
        
        /// <summary>
        /// The object for which the editor is being drawn.
        /// </summary>
        private OrdinalColorMatcher _target;
        
        /// <summary>
        /// The generator for retrieving swatches.
        /// </summary>
        private SwatchGenerator _swatchGenerator;
        
        /// <summary>
        /// All the properties of the target.
        /// </summary>
        private OrdinalColorMatcherPropertyCache _propertyCache;
        
        /// <summary>
        /// Texture cache for displaying in the editor.
        /// </summary>
        private ColorPaletteManagementTextures _textureCache;
        
        /// <summary>
        /// Style cache for custom styles.
        /// </summary>
        private ColorPaletteManagementStyleCache _styleCache;

        /// <summary>
        /// Initialize members.
        /// </summary>
        protected override void OnEnable()
        {
            _propertyCache = new OrdinalColorMatcherPropertyCache(serializedObject);
            _styleCache = new ColorPaletteManagementStyleCache();
            _textureCache = new ColorPaletteManagementTextures();
            _swatchGenerator = new SwatchGenerator();
            _target = (OrdinalColorMatcher)target; 
            base.OnEnable();
        }

        /// <summary>
        /// Draw the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            GUIContent centralColorPaletteContent = new GUIContent(_propertyCache.CentralColorPalette.displayName, _propertyCache.CentralColorPalette.tooltip);
            EditorGUILayout.PropertyField(_propertyCache.CentralColorPalette, centralColorPaletteContent);
            GUIContent overrideAlphaContent = new GUIContent(_propertyCache.OverrideAlpha.displayName, _propertyCache.OverrideAlpha.tooltip);
            EditorGUILayout.PropertyField(_propertyCache.OverrideAlpha, overrideAlphaContent);
            DrawOrdinalSelector();
            ColorMatchUtility.DrawColorMatchPreview(_target);

            // Display help box if object is not assigned.
            if (_target.CentralColorPalette == null)
                EditorGUILayout.HelpBox("Make sure to set the Central Color Palette", MessageType.Error);
            
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws the dropdown selector for the desired ordinal color.
        /// </summary>
        private void DrawOrdinalSelector()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUIContent ordinalLabelContent = new GUIContent(_propertyCache.Ordinal.displayName, _propertyCache.Ordinal.tooltip);
                EditorGUILayout.PrefixLabel(ordinalLabelContent);
                GUILayout.Space(2);
                if (GUILayout.Button(_textureCache.Palette, _styleCache.IconButton, GUILayout.Width(18), GUILayout.Height(18)) && _target.CentralColorPalette != null)
                {
                    _dropdown = CreateExtendedDropdown(new Rect(new Vector2(0, 0), new Vector2(Screen.width, 0)));
                    _dropdown.Show();
                }

                // Define standard name and color.
                string colorName = "No Central Color Palette assigned";
                Color color = Color.magenta;
                
                // Overwrite if the central color palette is not null.
                if (_target.CentralColorPalette != null)
                {
                    Palette palette = _target.CentralColorPalette.SelectedPalette;
                    if (palette != null && palette.Count > 0)
                        color = palette[Mathf.Min((int)_target.Ordinal, palette.Count - 1)].Color;

                    colorName = GetNameForOrdinal(_target.Ordinal, true);
                }

                GUIStyle style = new GUIStyle(GUI.skin.label) { richText = true };
                EditorGUILayout.LabelField(new GUIContent(colorName, _swatchGenerator.Generate(color)), style, GUILayout.Height(14));
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Creates the dropdown for the colors.
        /// </summary>
        /// <param name="rect">Where to place the dropdown.</param>
        /// <returns>The created dropdown.</returns>
        private ExtendedDropdown CreateExtendedDropdown(Rect rect)
        {
            ExtendedDropdownBuilder builder = new ExtendedDropdownBuilder("Color Ordinal", rect, new AdvancedDropdownState());
            Ordinal[] ordinals = Enum.GetValues(typeof(Ordinal)).Cast<Ordinal>().ToArray();
            Palette palette = _target.CentralColorPalette.SelectedPalette;
            if (palette == null)
                return builder.GetResult();
            for (int i = 0; i < ordinals.Length; i++)
            {
                string name = GetNameForOrdinal(ordinals[i], false);
                Color color = Color.magenta;
                if(palette.Count > 0)
                    color = (int)ordinals[i] < palette.Count ? palette[(int)ordinals[i]].Color : palette.Last().Color;

                int _i = i;
                builder.AddItem(name, _target.Ordinal == ordinals[i], _swatchGenerator.Generate(color), () => OnOrdinalSelected((Ordinal)_i));
            }

            return builder.GetResult();
        }

        /// <summary>
        /// Called when an ordinal is selected.
        /// </summary>
        /// <param name="ordinal">The ordinal that was selected.</param>
        private void OnOrdinalSelected(Ordinal ordinal)
        {
            _propertyCache.Ordinal.enumValueIndex = (int) ordinal;
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Retrieves the display name for the given ordinal.
        /// </summary>
        /// <param name="ordinal">The ordinal to get the display name for.</param>
        /// <param name="useRichText">Whether to make use of rich text.</param>
        /// <returns>The display name for the given ordinal.</returns>
        private string GetNameForOrdinal(Ordinal ordinal, bool useRichText)
        {
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append(ordinal.MapReadableName());
            Palette palette = _target.CentralColorPalette.SelectedPalette;
            nameBuilder.Append(" - ");
            if (palette == null)
            {
                nameBuilder.Append("Missing reference");
            }
            else if (palette.Count > 0)
            {
                PaletteColor color = (int)ordinal < palette.Count ? palette[(int)ordinal] : palette.Last();
                nameBuilder.Append(useRichText ? color.Name.Bold() : color.Name);
            }
            else
            {
                nameBuilder.Append("Palette has no colors");
            }

            return nameBuilder.ToString();
        }
    }
}