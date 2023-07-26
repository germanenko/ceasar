using UnityEngine;
using UnityEngine.UI;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Handles applying the color to a <see cref="Graphic"/> object.
    /// </summary>
    internal class GraphicColorApplier : IColorApplier
    {
        /// <summary>
        /// The graphic object to apply the color to.
        /// </summary>
        private readonly Graphic _graphic;

        /// <summary>
        /// Creates a new instance of the color applier.
        /// </summary>
        /// <param name="graphic">The graphic to apply the color to.</param>
        public GraphicColorApplier(Graphic graphic) => _graphic = graphic;

        /// <summary>
        /// Applies the color to the graphic.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        /// <param name="overrideAlpha">Whether to override the alpha.</param>
        public void Apply(Color color, bool overrideAlpha) 
        {
            _graphic.color =
                ColorPaletteManagerUtilities.ApplyColor(_graphic.color, color, overrideAlpha);
        }
    }
}