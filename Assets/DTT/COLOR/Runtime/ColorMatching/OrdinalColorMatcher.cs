#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Applies color based on ordinality (primary/secondary/etc.), instead of a direct reference.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("Color Management/Ordinal Color Matcher")]
    public class OrdinalColorMatcher : MonoBehaviour
    {
        /// <summary>
        /// The ordinality to match to.
        /// </summary>
        [SerializeField]
        [Tooltip("The ordinal index (primary/secondary/etc.) to apply color, instead of a direct reference to a color.")]
        private Ordinal _ordinal;

        /// <summary>
        /// The central color palette to use for the palette reference.
        /// </summary>
        [SerializeField]
        [Tooltip("The centralized component for setting your color palette.")]
        private CentralColorPalette _centralColorPalette;
        
        /// <summary>
        /// Whether to ignore the alpha value.
        /// </summary>
        [SerializeField]
        [Tooltip("Whether to override the alpha of your components.")]
        private bool _overrideAlpha = true;
        
        /// <summary>
        /// The central color palette to use for the palette reference.
        /// </summary>
        public CentralColorPalette CentralColorPalette => _centralColorPalette;
        
        /// <summary>
        /// The ordinality to match to.
        /// </summary>
        public Ordinal Ordinal
        {
            get => _ordinal;
            set => _ordinal = value;
        }

        /// <summary>
        /// Helps with detecting components and applying colors.
        /// </summary>
        private ComponentColorApplier _componentColorApplier;
        
        /// <summary>
        /// The color being used.
        /// </summary>
        private PaletteColor _current;
        
        #if UNITY_EDITOR
        /// <summary>
        /// Retrieves the component.
        /// </summary>
        private void OnValidate()
        {
            if(_componentColorApplier == null)
                _componentColorApplier = new ComponentColorApplier(this);
            _componentColorApplier.Refresh();
            OnDisable();
            OnEnable();

            EditorSceneManager.sceneOpened -= ApplyColor;
            EditorSceneManager.sceneOpened += ApplyColor;
            
            void ApplyColor(Scene scene, OpenSceneMode mode)
            {
                if (this == null)
                    return;
                this.ApplyColor();
            }
            
            this.ApplyColor();
        }
        #endif

        /// <summary>
        /// Sets up the component to be able to match colors.
        /// </summary>
        private void OnEnable()
        {
            if (_centralColorPalette == null || _centralColorPalette.SelectedPalette == null)
                return;

            _componentColorApplier = new ComponentColorApplier(this);

            if (_centralColorPalette.SelectedPalette[_ordinal] != null)
            {
                _current = _centralColorPalette.SelectedPalette[_ordinal];
                _current.Updated += ApplyColor;
            }
            _centralColorPalette.Selection.Updated += OnPaletteUpdated;
        }
        
        /// <summary>
        /// Cleans up the component from being able to match colors.
        /// </summary>
        private void OnDisable()
        {
            if (_centralColorPalette == null || _centralColorPalette.SelectedPalette == null)  
                return;
            
            if(_centralColorPalette.SelectedPalette[_ordinal] != null)
                _centralColorPalette.SelectedPalette[_ordinal].Updated -= ApplyColor;
            _centralColorPalette.Selection.Updated -= OnPaletteUpdated;
        }

        /// <summary>
        /// When the palette is updated we clean up and reapply our events.
        /// </summary>
        private void OnPaletteUpdated()
        {
            _current.Updated -= ApplyColor;
            if (_centralColorPalette.SelectedPalette[_ordinal] != null)
            {
                _current = _centralColorPalette.SelectedPalette[_ordinal];
                _current.Updated += ApplyColor;
            }
            
            ApplyColor(); 
        }

        /// <summary>
        /// Applies the color to the components only if we have correct references.
        /// </summary>
        private void ApplyColor()
        {
            if (_centralColorPalette == null || _centralColorPalette.SelectedPalette == null)
                return;
            _componentColorApplier.Apply(_centralColorPalette.SelectedPalette[_ordinal].Color, _overrideAlpha);
        }
    }
}