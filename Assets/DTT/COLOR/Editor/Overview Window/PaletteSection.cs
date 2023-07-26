using System;
using System.Collections.Generic;
using System.Linq;
using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor.OverviewEditor
{
    /// <summary>
    /// Represents a palette containing all the colors.
    /// </summary>
    internal class PaletteSection : IDrawable
    {
        /// <summary>
        /// Called when something was mutated or changed.
        /// </summary>
        public event Action Mutated;
        
        /// <summary>
        /// Called when the delete button is pressed.
        /// </summary>
        public event Action DeleteButtonPressed;

        /// <summary>
        /// Invoked when a swap button action occurs, passing the indices of the colors that should be swapped. 
        /// </summary>
        public event Action<int, int> Swap;
        
        /// <summary>
        /// The foldout used for showing the colors.
        /// </summary>
        private readonly AnimatedFoldout _foldout;
        
        /// <summary>
        /// The backing palette.
        /// </summary>
        public readonly Palette palette;
        
        /// <summary>
        /// The textures used in this package.
        /// </summary>
        private readonly ColorPaletteManagementTextures _textures;

        /// <summary>
        /// All the sections for the colors.
        /// </summary>
        private List<PaletteColorSection> _paletteColorSections;

        /// <summary>
        /// The styles used in this window.
        /// </summary>
        private readonly ColorPaletteManagementStyleCache _styleCache;

        /// <summary>
        /// Creates a new palette section.
        /// </summary>
        /// <param name="palette">The backing palette.</param>
        /// <param name="editor">The window this is placed in.</param>
        /// <param name="textures">The textures used in this package.</param>
        /// <param name="styleCache">The styles used in this window.</param>
        public PaletteSection(Palette palette, EditorWindow editor, ColorPaletteManagementTextures textures, ColorPaletteManagementStyleCache styleCache)
        {
            _foldout = new AnimatedFoldout(editor, true);
            this.palette = palette;
            _textures = textures;
            _styleCache = styleCache;

            GenerateColorSections();

            Mutated += OnMutated;
        }

        /// <summary>
        /// Called when something is mutated.
        /// </summary>
        private void OnMutated()
        {
            // Regenerates all the buttons.
            // This makes sure all colors show up.
            GenerateColorSections();
        }

        /// <summary>
        /// Called when the add button is pressed for a new palette.
        /// </summary>
        private void OnAddButtonPressed()
        {
            palette.Add(new PaletteColor());
            
            Mutated?.Invoke();
        }

        /// <summary>
        /// Called when the delete button is pressed.
        /// </summary>
        /// <param name="color">The color needing to be removed.</param>
        private void OnDeleteButtonPressed(PaletteColor color)
        {
            const string TITLE = "Color Palette Manger Warning";
            const string MESSAGE = "Are you sure you want to remove the color \"{0}\" in \"{1}\"?";
            const string OK = "Delete";
            const string CANCEL = "Cancel";
            bool confirmed = EditorUtility.DisplayDialog(TITLE, string.Format(MESSAGE, color.Name, palette.Name), OK, CANCEL);
            if (!confirmed)
                return;
            
            palette.Remove(color);
            Mutated?.Invoke();
        }

        /// <summary>
        /// Draws the palette section.
        /// </summary>
        public void Draw()
        {
            _foldout.OnGUI(palette.Name, () =>
            {
                EditorGUILayout.Space(8);
                EditorGUI.indentLevel++;
                {
                    if(palette.Count == 0)
                        EditorGUILayout.LabelField("Try adding a color by using the field below!", _styleCache.ItalicLabel);
                
                    foreach (var drawable in _paletteColorSections)
                    {
                        drawable.Draw();

                        if (drawable != _paletteColorSections.LastOrDefault())
                            EditorGUILayout.Space(3);
                    }
                    EditorGUILayout.Space(12);

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(18);
                        GUIContent addButtonContent = new GUIContent(_textures.Insert, $"Adds a new color to the \"{palette.Name}\" palette.");
                        if(GUILayout.Button(addButtonContent, _styleCache.IconButton, GUILayout.Height(18), GUILayout.Width(38)))
                            OnAddButtonPressed();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }, DrawHeaderButtons);
        }

        /// <summary>
        /// Creates a popup allowing the user to rename the popup.
        /// </summary>
        /// <param name="rect">Where to place the popup.</param>
        private void CreateRenamePopup(Rect rect)
        {
            RenamePopup popup = new RenamePopup(palette.Name);
            popup.Renamed += OnRenamed;
            PopupWindow.Show(rect, popup);

            void OnRenamed(string value)
            {
                palette.Name = value;
                Mutated?.Invoke();
            }
        }

        /// <summary>
        /// Draws the header buttons on the palette section.
        /// </summary>
        /// <param name="foldoutRect">The rect of the foldout.</param>
        private void DrawHeaderButtons(Rect foldoutRect)
        {
            Rect renameButtonRect = new Rect(foldoutRect)
            {
                position = new Vector2(foldoutRect.width - 14 - 18 - 5, foldoutRect.y),
                size = new Vector2(18, 18)
            };

            GUIContent renameButtonContent = new GUIContent(_textures.Edit, $"Edit the name of the \"{palette.Name}\" palette");
            if (GUI.Button(renameButtonRect, renameButtonContent, _styleCache.IconButton))
                CreateRenamePopup(renameButtonRect);

            Rect deleteButtonRect = new Rect(foldoutRect)
            {
                position = new Vector2(foldoutRect.width - 14, foldoutRect.y),
                size = new Vector2(18, 18)
            };
            
            GUIContent deleteButtonContent = new GUIContent(_textures.TrashBin, $"Delete the \"{palette.Name}\" palette");
            if (GUI.Button(deleteButtonRect, deleteButtonContent, _styleCache.IconButton))
                DeleteButtonPressed?.Invoke();
        }

        /// <summary>
        /// Generates the UI elements for the colors.
        /// </summary>
        private void GenerateColorSections()
        {
            _paletteColorSections = new List<PaletteColorSection>();
            for (int i = 0; i < palette.Count; i++)
            {
                // Capture variable.
                int _i = i;
                PaletteColorSection section = new PaletteColorSection(palette[i], _textures, _styleCache);
                section.ButtonPressed += () => OnDeleteButtonPressed(palette[_i ]);
                section.UpButtonPressed += () => OnUpButtonPressed(_i );
                section.DownButtonPressed += () => OnDownButtonPressed(_i );
                _paletteColorSections.Add(section);
            }
        }

        /// <summary>
        /// Called when the up button is pressed on a color section, passing the index of itself.
        /// </summary>
        /// <param name="index">The index of the element where the up button was pressed.</param>
        private void OnUpButtonPressed(int index)
        {
            if (index == 0)
                return;
            Swap?.Invoke(index, index - 1);
        }
        
        /// <summary>
        /// Called when the down button is pressed on a color section, passing the index of itself.
        /// </summary>
        /// <param name="index">The index of the element where the down button was pressed.</param>
        private void OnDownButtonPressed(int index)
        {
            if (index == palette.Count - 1)
                return;
            
            Swap?.Invoke(index, index + 1);
        }
    }
}