using UnityEngine;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Handles applying the color to a <see cref="Camera"/> object.
    /// </summary>
    internal class CameraColorApplier : IColorApplier
    {
        /// <summary>
        /// The camera object to apply the color to.
        /// </summary>
        private readonly Camera _camera;

        /// <summary>
        /// Creates a new instance of the color applier.
        /// </summary>
        /// <param name="camera">The camera to apply the color to.</param>
        public CameraColorApplier(Camera camera) => _camera = camera;

        /// <summary>
        /// Applies the color to the graphic.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        /// <param name="overrideAlpha">Whether to override the alpha.</param>
        public void Apply(Color color, bool overrideAlpha)
        {
            _camera.backgroundColor = ColorPaletteManagerUtilities.ApplyColor(_camera.backgroundColor, color, overrideAlpha);
        }
    }
}