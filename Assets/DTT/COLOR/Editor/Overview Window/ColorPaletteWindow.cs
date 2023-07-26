using System.Collections.Generic;
using System.Linq;
using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor.OverviewEditor
{
    /// <summary>
    /// Draws the overview window for the color palette manager.
    /// </summary>
    [DTTHeader("dtt.color-palette-manager")]
    internal class ColorPaletteWindow : DTTEditorWindow
    {
        /// <summary>
        /// Reference to the database containing all the palettes.
        /// This is the main source of data for this class.
        /// </summary>
        private PaletteDatabase _database;
        
        /// <summary>
        /// The different sections for the palettes that have to be drawn.
        /// </summary>
        private List<PaletteSection> _paletteSections;

        /// <summary>
        /// The textures using in this window.
        /// </summary>
        private ColorPaletteManagementTextures _textures;
        
        /// <summary>
        /// The styles used in this package.
        /// </summary>
        private ColorPaletteManagementStyleCache _styleCache;

        /// <summary>
        /// Generates the code based on the palette data.
        /// </summary>
        private PaletteCodeGenerator _paletteCodeGenerator;
        
        /// <summary>
        /// The current scroll position of the palettes view.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// The name used for a new palette.
        /// </summary>
        private string _newPaletteName = NEW_PALETTE_DEFAULT;
        
        /// <summary>
        /// The default new palette name.
        /// </summary>
        private const string NEW_PALETTE_DEFAULT = "New Palette";

        /// <summary>
        /// The path to export the generated file to.
        /// </summary>
        private string _exportPath
        {
            get => EditorPrefs.GetString(Application.productName + "-CPM_ExportPath", Application.dataPath);
            set => EditorPrefs.SetString(Application.productName + "-CPM_ExportPath", value);
        }

        /// <summary>
        /// Whether a mutation has happened and we have to refresh our data.
        /// </summary>
        private bool _isMutated;

        /// <summary>
        /// Foldout for the header, containing explanation for generating source.
        /// </summary>
        private AnimatedFoldout _headerFoldout;

        /// <summary>
        /// Displays the window.
        /// </summary>
        [MenuItem("Tools/DTT/COLOR/Color Palette Window")]
        internal static void ShowWindow()
        {
            ColorPaletteWindow window = GetWindow<ColorPaletteWindow>();
            if (window == null)
                return;
            window.titleContent = new GUIContent("Color Palette Window");
            window.Show();
        }

        /// <summary>
        /// Constructs all required dependencies.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _textures = new ColorPaletteManagementTextures();
            _styleCache = new ColorPaletteManagementStyleCache();
            _database = AssetDatabaseUtility.LoadAsset<PaletteDatabase>();
            _paletteCodeGenerator = new PaletteCodeGenerator(_database);
            _headerFoldout = new AnimatedFoldout(this);

            CreateSections();
            minSize = new Vector2(400, 300);
        }

        /// <summary>
        /// Creates all the sections for the palettes.
        /// </summary>
        private void CreateSections() => _paletteSections = new List<PaletteSection>(_database.Palettes.Select(CreateSection));

        /// <summary>
        /// Creates a section and tracks its events.
        /// </summary>
        /// <param name="palette">On what palette it is based.</param>
        /// <returns>The created section.</returns>
        private PaletteSection CreateSection(Palette palette)
        {
            PaletteSection section = new PaletteSection(palette, this, _textures, _styleCache);
            section.DeleteButtonPressed += () => OnDeleteButtonPressed(palette);
            section.Swap += (a, b) => OnSwap(palette, a, b);
            return section;
        }

        /// <summary>
        /// Called when a swap has occured.
        /// </summary>
        /// <param name="palette">The palette on which the swap happens.</param>
        /// <param name="a">The first element being swapped.</param>
        /// <param name="b">The second element being swapped.</param>
        private void OnSwap(Palette palette, int a, int b)
        {
            // Using object deconstruction to perform a variable swap.
            (palette[a], palette[b]) = (palette[b], palette[a]);
            foreach (PaletteColor color in palette)
                color.Refresh();
            
            _isMutated = true;
            CreateSections();
        }

        /// <summary>
        /// Called when the delete button of a palette is pressed.
        /// </summary>
        /// <param name="palette">The palette wanting to be deleted.</param>
        private void OnDeleteButtonPressed(Palette palette)
        {
            const string TITLE = "Color Palette Manger Warning";
            const string MESSAGE = "Are you sure you want to remove the palette \"{0}\"?";
            const string OK = "Delete";
            const string CANCEL = "Cancel";
            bool confirmed = EditorUtility.DisplayDialog(TITLE, string.Format(MESSAGE, palette.Name), OK, CANCEL);
            if (!confirmed)
                return;
            
            _database.Remove(palette);
            _paletteSections.Remove(_paletteSections.First(section => section.palette == palette));

            _isMutated = true;
        }

        /// <summary>
        /// Draws the window.
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();
            EditorGUI.BeginChangeCheck();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, _styleCache.ScrollView);
            {
                DrawHeader();

                EditorGUILayout.Space();
                
                // Draw the sections.
                foreach (IDrawable drawable in _paletteSections)
                {
                    drawable.Draw();
                    if(drawable != _paletteSections.LastOrDefault())
                        EditorGUILayout.Space(12);
                    if (!_isMutated)
                        continue;
                    
                    // To prevent the collection from being modified during draws.
                    _isMutated = false;   
                    break;
                }

                if (_paletteSections.Count == 0)
                    EditorGUILayout.LabelField("Try adding a palette using the field below!", _styleCache.ItalicLabel);
            }

            EditorGUILayout.Space(18);
            GUILayout.FlexibleSpace();
            
            DrawPaletteInputField();

            EditorGUILayout.EndScrollView();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_database);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Draws the header. Showing where to export the source.
        /// </summary>
        private void DrawHeader()
        {
            _headerFoldout.OnGUI("Source export", () =>
            {
                EditorGUILayout.HelpBox("Pressing the save button will export a C# source file that you can reference from within your code to be able to make use of your defined colors.", MessageType.Info);
            });
            GUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.TextField(_exportPath);
                }
                EditorGUI.EndDisabledGroup();

                GUIContent folderContent = new GUIContent(EditorGUIUtility.IconContent("Folder Icon").image, "Select the export folder for the source code file.");
                if (GUILayout.Button(folderContent, _styleCache.IconButton, GUILayout.Width(22), GUILayout.Height(22)))
                {
                    string newPath = EditorUtility.OpenFolderPanel("Export path", _exportPath, string.Empty);
                    if (newPath != _exportPath && newPath != string.Empty)
                    {
                        const string TITLE = "COLOR Warning";
                        const string MESSAGE = "Are you sure you want to change your export directory? If so make sure to remove the old file after generating to make sure no two instances of the script will be in your project.";
                        const string OK = "Ok";
                        const string CANCEL = "Cancel";
                        bool @continue = EditorUtility.DisplayDialog(TITLE, MESSAGE, OK, CANCEL);
                        if (!@continue)
                            return;
                        _exportPath = newPath;
                    }
                }

                GUIContent saveContent = new GUIContent(_textures.Save, "Saves a generated source code file of your palettes and colors to the path you defined.");
                if (GUILayout.Button(saveContent, _styleCache.IconButton, GUILayout.Width(22), GUILayout.Height(22)))
                {
                    AssetDatabase.SaveAssets();
                    _paletteCodeGenerator.GenerateCode(_exportPath);
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the input field for creating new palettes.
        /// </summary>
        private void DrawPaletteInputField()
        {
            DTTGUILayout.CardHeader(() =>
            {
                EditorGUILayout.BeginHorizontal(new GUIStyle {margin = new RectOffset(0, 0, 0, 3)});
                {
                    EditorGUILayout.LabelField("Palette", GUILayout.Width(68));
                    _newPaletteName = EditorGUILayout.TextField(_newPaletteName, GUILayout.Width(Screen.width * 0.5f - 15));
                    GUILayout.FlexibleSpace();
                    GUIContent addPaletteButtonContent = new GUIContent(_textures.Insert, "Adds a new palette");
                    if (GUILayout.Button(addPaletteButtonContent, new GUIStyle(_styleCache.IconButton) { padding = new RectOffset(2, 2, 2, 2) }, GUILayout.Width(18), GUILayout.Height(18)))
                    {
                        Palette palette = new Palette { Name = _newPaletteName };
                        _database.Add(palette);
                        PaletteSection section = CreateSection(palette);
                        _paletteSections.Add(section);

                        _isMutated = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
            });
        }
    }
}