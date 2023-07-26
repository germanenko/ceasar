using System.Collections.Generic;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Helps with generating swatch icons.
    /// </summary>
    internal class SwatchGenerator
    {
        /// <summary>
        /// Caches generated icons.
        /// </summary>
        private readonly Dictionary<Color, Texture2D> _cache = new Dictionary<Color, Texture2D>();

        /// <summary>
        /// Generates an icon for the given color.
        /// </summary>
        /// <param name="color">Color to make the swatch for.</param>
        /// <returns>A texture icon.</returns>
        public Texture2D Generate(Color color)
        {
            // If in cache retrieve from there.
            if (_cache.ContainsKey(color))
                return _cache[color];
            
            Texture2D texture = GenerateTemplate();

            // Fill with main color in centre.
            Color[] colors = new Color[9 * 9];
            for (int j = 0; j < 9 * 9; j++)
                colors[j] = color;

            texture.SetPixels(2, 2, 9, 9, colors);
            texture.Apply();

            // Add to cache.
            _cache.Add(color, texture);
            
            return texture;
        }

        /// <summary>
        /// Generates a base texture that can be used for swatches where only the color itself has to be filled in.
        /// </summary>
        /// <returns>The generated template.</returns>
        private Texture2D GenerateTemplate()
        {
            Texture2D texture = new Texture2D(13, 13);

            // Add padding.
            for (int j = 0; j < 13; j++)
            {
                texture.SetPixel(0, j, Color.clear);
                texture.SetPixel(12, j, Color.clear);
            }
            // Add padding.
            for (int j = 1; j < 12; j++)
            {
                texture.SetPixel(j, 0, Color.clear);
                texture.SetPixel(j, 12, Color.clear);
            }

            // Set to point filter mode, for better clarity.
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            
            return texture;
        }
    }
}