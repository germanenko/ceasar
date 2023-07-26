#if UNITY_EDITOR

using DTT.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// The type selector content to show in which the user can select a type.
    /// </summary>
    public class TypeSelectorContent : PopupWindowContent
    {
        /// <summary>
        /// Holds the GUI content used by the selector.
        /// </summary>
        private class ContentCache : GUIContentCache
        {
            /// <summary>
            /// The content for an assembly button.
            /// </summary>
            public GUIContent AssemblyButton => base[nameof(AssemblyButton)];

            /// <summary>
            /// The content for a type button.
            /// </summary>
            public GUIContent TypeButton => base[nameof(TypeButton)];

            /// <summary>
            /// Initializes the gui content.
            /// </summary>
            public ContentCache()
            {
                Add(nameof(AssemblyButton), () => new GUIContent(EditorGUIUtility.IconContent("AssemblyDefinitionAsset Icon")));
                Add(nameof(TypeButton), () => new GUIContent(EditorGUIUtility.IconContent("cs Script Icon")));
            }
        }

        /// <summary>
        /// The gui styles used by the type selector.
        /// </summary>
        private class StyleCache : GUIStyleCache
        {
            /// <summary>
            /// A gui style for a list button.
            /// </summary>
            public GUIStyle ListButton => base[nameof(ListButton)];

            /// <summary>
            /// A gui style for a back button.
            /// </summary>
            public GUIStyle BackButton => base[nameof(BackButton)];

            /// <summary>
            /// Initializes the gui styles.
            /// </summary>
            public StyleCache()
            {
                Add(nameof(ListButton), () =>
                {
                    GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
                    style.alignment = TextAnchor.MiddleLeft;
                    return style;
                });

                Add(nameof(BackButton), () =>
                {
                    GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
                    style.fontStyle = FontStyle.Bold;
                    return style;
                });
            }
        }

        /// <summary>
        /// The clicked method.
        /// Used for returning the type back to the using script.
        /// </summary>
        private Action<string> _clicked;

        /// <summary>
        /// The main filter type.
        /// </summary>
        private Type _filter;

        /// <summary>
        /// The gui styles used for drawing the selector.
        /// </summary>
        private StyleCache _styles;

        /// <summary>
        /// The gui content used for drawing the selector.
        /// </summary>
        private ContentCache _content;

        /// <summary>
        /// The string of the filter.
        /// </summary>
        private string _textFilter = string.Empty;

        /// <summary>
        /// The height of the window.
        /// </summary>
        private const float HEIGHT = 300;

        /// <summary>
        /// The size of the window.
        /// </summary>
        private readonly Vector2 _size;

        /// <summary>
        /// The scroll position of the elements.
        /// </summary>
        private Vector2 _currentScrollPosition;

        /// <summary>
        /// The currently selected Assembly.
        /// </summary>
        private Assembly _currentSelectedAssembly;

        /// <summary>
        /// The internal filter assembly data.
        /// </summary>
        private Dictionary<Assembly, Type[]> _assemblyData = new Dictionary<Assembly, Type[]>();

        /// <summary>
        /// A list of globally filtererd assemblies.
        /// </summary>
        private Assembly[] _assemblies;

        /// <summary>
        /// The filtered assembly with types.
        /// </summary>
        private Dictionary<Assembly, Type[]> _assemblyDataFiltered = new Dictionary<Assembly, Type[]>();

        /// <summary>
        /// the filtered assembly.
        /// </summary>
        private Assembly[] _assembliesFiltered;
        
        /// <summary>
        /// The popup type.
        /// </summary>
        /// <param name="clicked">The method to call when a type has been clicked.</param>
        /// <param name="filter">The type used to filter in the selector.</param>
        public TypeSelectorContent(Type filter, float width, Action<string> clicked)
        {
            _filter = filter;
            _clicked = clicked;
            _styles = new StyleCache();
            _content = new ContentCache();
            _size = new Vector2(width, HEIGHT);
        }

        /// <summary>
        /// Gets the size of the window.
        /// </summary>
        /// <returns>The size of the window.</returns>
        public override Vector2 GetWindowSize() => _size;

        /// <summary>
        /// Draws the popup contents within the given space.
        /// </summary>
        /// <param name="rect">The given space of the content.</param>
        public override void OnGUI(Rect rect)
        {
            DrawSearchField();

            _currentScrollPosition = EditorGUILayout.BeginScrollView(_currentScrollPosition);
            if (_currentSelectedAssembly == null)
            {
                // If not assembly has been selected, draw the filtered assemblies.
                DrawFilteredAssemblies();
            }
            else
            {
                bool clickedButton = GUILayout.Button("Back", _styles.BackButton, GUILayout.Width(_size.x));

                // If an assembly has been selected, draw its types.
                DrawSelectedAssemblyTypes();

                // Reset the currently selected assembly if the back button has been clicked.
                if (clickedButton)
                    _currentSelectedAssembly = null;
            }

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Initializes filtered assemblies to be shown.
        /// </summary>
        public override void OnOpen()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> filtered = new List<Assembly>();

            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                List<Type> filteredTypes = new List<Type>();
                AddFilteredTypes(types, filteredTypes);

                if (filteredTypes.Count != 0)
                {
                    filtered.Add(assemblies[i]);
                    AddAssemblyData(assemblies[i], filteredTypes);
                }
            }

            AddFilteredAssemblies(filtered);
        }

        /// <summary>
        /// Draws a searchfield and updates the filtered assemblies based on the result.
        /// </summary>
        private void DrawSearchField()
        {
            _textFilter = GUIDrawTools.SearchField(_textFilter, out bool updated);

            if (updated)
                FilterFromText();
        }

        /// <summary>
        /// Draws buttons for filtered assemblies.
        /// </summary>
        private void DrawFilteredAssemblies()
        {
            for (int i = 0; i < _assembliesFiltered.Length; i++)
            {
                GUIContent content = new GUIContent(_content.AssemblyButton);
                content.text = _assembliesFiltered[i].GetName().Name;
                if (GUILayout.Button(content, _styles.ListButton))
                    _currentSelectedAssembly = _assembliesFiltered[i];
            }
        }

        /// <summary>
        /// Draws the types inside the currently selected assembly.
        /// </summary>
        private void DrawSelectedAssemblyTypes()
        {
            Type[] types = _assemblyDataFiltered[_currentSelectedAssembly];
            for (int i = 0; i < types.Length; i++)
            {
                GUIContent content = new GUIContent(_content.TypeButton);
                content.text = types[i].Name;
                if (GUILayout.Button(content, _styles.ListButton, GUILayout.Width(_size.x)))
                {
                    _clicked(types[i].AssemblyQualifiedName);
                    editorWindow.Close();
                }
            }
        }

        /// <summary>
        /// Filters the types from the filter text.
        /// </summary>
        private void FilterFromText()
        {
            if (!string.IsNullOrEmpty(_textFilter))
            {
                _assemblyDataFiltered = new Dictionary<Assembly, Type[]>();
                List<Assembly> filteredAssemblys = new List<Assembly>();
                List<Type> filteredTypes = new List<Type>();
                for (int i = 0; i < _assemblies.Length; i++)
                {
                    Type[] types = _assemblyData[_assemblies[i]];
                    for (int t = 0; t < types.Length; t++)
                        if (types[t].Name.ToLower().Contains(_textFilter))
                            filteredTypes.Add(types[t]);

                    if (filteredTypes.Count != 0)
                    {
                        _assemblyDataFiltered.Add(_assemblies[i], filteredTypes.ToArray());
                        filteredAssemblys.Add(_assemblies[i]);
                        filteredTypes.Clear();
                    }
                }
                _assembliesFiltered = filteredAssemblys.ToArray();
            }
            else
            {
                _assemblyDataFiltered = _assemblyData;
                _assembliesFiltered = _assemblies;
            }
        }

        /// <summary>
        /// Will add types from given types array to the filtered array if they come through the filter. 
        /// </summary>
        /// <param name="types">The types to check on.</param>
        /// <param name="filtered">The list to update with filtered types.</param>
        private void AddFilteredTypes(Type[] types, List<Type> filtered)
        {
            if (_filter.IsInterface)
            {
                for (int i = 0; i < types.Length; i++)
                    if ((types[i].ImplementsInterface(_filter) && !types[i].IsInterface))
                        filtered.Add(types[i]);
            }
            else
            {
                for (int i = 0; i < types.Length; i++)
                    if (types[i].IsSubclassOf(_filter))
                        filtered.Add(types[i]);
            }
        }

        /// <summary>
        /// Adds assembly data with given filtered types to the assembly data dictionary.
        /// </summary>
        /// <param name="assemby">The assembly to add.</param>
        /// <param name="filtered">The filtered list of types.</param>
        private void AddAssemblyData(Assembly assemby, List<Type> filtered)
        {
            filtered.Sort((left, right) => string.Compare(left.Name, right.Name));

            _assemblyData.Add(assemby, filtered.ToArray());
        }

        /// <summary>
        /// Assigns filtered assembly data to stored state.
        /// </summary>
        /// <param name="filtered">The filtered assemblies to assign.</param>
        private void AddFilteredAssemblies(List<Assembly> filtered)
        {
            filtered.Sort((a, b) => string.Compare(a.GetName().Name, b.GetName().Name));

            _assemblies = filtered.ToArray();
            _assemblyDataFiltered = _assemblyData;
            _assembliesFiltered = _assemblies;
        }
    }
}

#endif
