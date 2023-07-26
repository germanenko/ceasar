using UnityEngine;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Defines the central point for making a palette selection for <see cref="OrdinalColorMatcher"/>.
    /// </summary>
    [AddComponentMenu("Color Management/Central Color Palette")]
    public class CentralColorPalette : MonoBehaviour
    {
        /// <summary>
        /// The palette that is selected.
        /// </summary>
        [SerializeField]
        [Tooltip("The palette to centrally use for Ordinal Color Matchers.")]
        private PaletteSelection _selectedPalette;

        /// <summary>
        /// The palette that is selected.
        /// </summary>
        public Palette SelectedPalette
        {
            get => _selectedPalette.Palette;
            set => _selectedPalette.Palette = value;
        }
        
        /// <summary>
        /// The selection object that serializes the palette selection.
        /// </summary>
        public PaletteSelection Selection => _selectedPalette;
    }
}