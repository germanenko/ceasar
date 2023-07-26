using UnityEngine;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Defines an interface to allows for applying colors to objects.
    /// </summary>
    public interface IColorApplier
    {
        /// <summary>
        /// Should apply the given color to the object.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        /// <param name="overrideAlpha">Whether to use the alpha from the color.</param>
        void Apply(Color color, bool overrideAlpha);
    }
}