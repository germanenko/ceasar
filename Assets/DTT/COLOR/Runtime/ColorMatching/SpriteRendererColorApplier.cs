using UnityEngine;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Handles applying the color to a <see cref="SpriteRenderer"/> object.
    /// </summary>
    internal class SpriteRendererColorApplier : IColorApplier
    {
        /// <summary>
        /// Handles applying the color to a <see cref="SpriteRenderer"/> object.
        /// </summary>
        private readonly SpriteRenderer _spriteRenderer;

        /// <summary>
        /// Creates a new instance of the color applier.
        /// </summary>
        /// <param name="spriteRenderer">The sprite renderer to apply the color to.</param>
        public SpriteRendererColorApplier(SpriteRenderer spriteRenderer) => _spriteRenderer = spriteRenderer;

        /// <summary>
        /// Applies the color to the sprite renderer.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        /// <param name="overrideAlpha">Whether to override the alpha.</param>
        public void Apply(Color color, bool overrideAlpha) 
        {
            _spriteRenderer.color =
                ColorPaletteManagerUtilities.ApplyColor(_spriteRenderer.color, color, overrideAlpha);
        }
    }
}