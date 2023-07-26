using DTT.COLOR.ColorMatching;
using DTT.PublishingTools;
using UnityEditor;

namespace DTT.COLOR.Editor.ColorMatching
{
    /// <summary>
    /// Provides the editor for the <see cref="ColorMatcher"/>.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColorMatcher))]
    [DTTHeader("dtt.color-palette-manager", "Color Matcher")]
    internal class ColorMatcherEditor : DTTInspector
    {
        /// <summary>
        /// The target instance.
        /// </summary>
        private ColorMatcher _colorMatcher;
        
        /// <summary>
        /// Retrieves target.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _colorMatcher = (ColorMatcher) target;
        }

        /// <summary>
        /// Draws color selector and provide info to what is being targeted.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
            ColorMatchUtility.DrawColorMatchPreview(_colorMatcher);
        }
    }
}