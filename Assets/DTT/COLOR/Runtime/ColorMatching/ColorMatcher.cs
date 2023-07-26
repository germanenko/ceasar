#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Component that matches the color selected with that on the GameObject.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("Color Management/Color Matcher")]
    [DisallowMultipleComponent]
    public class ColorMatcher : MonoBehaviour
    {
        /// <summary>
        /// The color that will be applied to your objects.
        /// </summary>
        [SerializeField]
        [Tooltip("The color that will be applied to your objects.")]
        private PaletteColorSelection _selectedColor = new PaletteColorSelection();

        /// <summary>
        /// Whether to match the alpha value as well.
        /// </summary>
        [SerializeField]
        [Tooltip("Whether to override the alpha of your components.")]
        private bool _overrideAlpha = true;

        /// <summary>
        /// The color that will be applied to your objects.
        /// </summary>
        public PaletteColorSelection Selection => _selectedColor;

        /// <summary>
        /// Helps applying the color to components.
        /// </summary>
        private ComponentColorApplier _componentColorApplier;
        
        #if UNITY_EDITOR
        /// <summary>
        /// Retrieves the component.
        /// </summary>
        private void OnValidate()
        {
            if(_componentColorApplier == null)
                _componentColorApplier = new ComponentColorApplier(this);
            _componentColorApplier.Refresh();

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
        /// Sets up the component to make sure colors are matched.
        /// </summary>
        private void OnEnable()
        {
            _componentColorApplier = new ComponentColorApplier(this);
            
            if(_selectedColor.PaletteColor != null)
                _selectedColor.PaletteColor.Updated += ApplyColor;
            _selectedColor.Updated += ApplyColor;
        }
        
        /// <summary>
        /// Cleans up the component to make sure colors are matched.
        /// </summary>
        private void OnDisable()
        {
            if(_selectedColor.PaletteColor != null)
                _selectedColor.PaletteColor.Updated -= ApplyColor;
            _selectedColor.Updated -= ApplyColor;
        }

        /// <summary>
        /// Applies the color to the components.
        /// </summary>
        private void ApplyColor()
        {
            if (_selectedColor.PaletteColor == null)
                return;
            
            _componentColorApplier.Apply(_selectedColor.PaletteColor.Color, _overrideAlpha);
        }
    }
}