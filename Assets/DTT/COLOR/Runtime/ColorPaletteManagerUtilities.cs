using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Useful utilities for the CPM package.
    /// </summary>
    public static class ColorPaletteManagerUtilities
    {
        /// <summary>
        /// Determines a color based on the override alpha flag.
        /// </summary>
        /// <param name="colorA">The original color.</param>
        /// <param name="colorB">The destination color.</param>
        /// <param name="overrideAlpha">Whether to ignore the alpha of the destination color.</param>
        /// <returns>The final color.</returns>
        public static Color ApplyColor(Color colorA, Color colorB, bool overrideAlpha) => overrideAlpha ? colorB : new Color(colorB.r, colorB.g, colorB.b, colorA.a);
    }
}