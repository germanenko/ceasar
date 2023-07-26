using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.COLOR.Editor.ColorMatching
{
    /// <summary>
    /// Provides editor utility methods for the package.
    /// </summary>
    public static class ColorMatchUtility
    {
        /// <summary>
        /// Draws the match previews for the components that are being matched with.
        /// </summary>
        /// <param name="colorMatcher">The component for which it is being drawn.</param>
        public static void DrawColorMatchPreview(Component colorMatcher)
        {
            EditorGUI.BeginDisabledGroup(true);
            bool matchableFound = false;

            if(TryDrawForComponent<Camera>(colorMatcher))
                matchableFound = true;
            if(TryDrawForComponent<SpriteRenderer>(colorMatcher))
                matchableFound = true;
            if(TryDrawForComponent<Graphic>(colorMatcher))
                matchableFound = true;
            
            EditorGUI.EndDisabledGroup();
            
            if(!matchableFound)
                EditorGUILayout.HelpBox("No support component found to match the color to", MessageType.Warning);
        }

        /// <summary>
        /// Tries to draw a matching element for the given generic.
        /// </summary>
        /// <param name="component">The component the match component should be on.</param>
        /// <typeparam name="T">The type of component to look for.</typeparam>
        /// <returns>Whether a component was found.</returns>
        private static bool TryDrawForComponent<T>(Component component) where T : Component
        {
            if (component.TryGetComponent(out T result))
            {
                EditorGUILayout.ObjectField("Matching to", result, typeof(T), true);
                return true;
            }

            return false;
        }
    }
}