#if UNITY_TESTFRAMEWORK

using NUnit.Framework;
using DTT.ColorPaletteManager.Editor;
using UnityEngine;

namespace DTT.COLOR.Tests.Editor
{
    /// <summary>
    /// Tests the swatch generator.
    /// </summary>
    public class SwatchGeneratorTests
    {
        /// <summary>
        /// Tests whether the generated texture has the color provided in it.
        /// </summary>
        [Test]
        public void GeneratedSwatchContainsTargetColor()
        {
            // Arrange.
            SwatchGenerator swatchGenerator = new SwatchGenerator();
            Color targetColor = Color.magenta;
            
            // Act.
            Texture2D swatch = swatchGenerator.Generate(targetColor);
            
            // Assert.
            Assert.AreEqual(targetColor, swatch.GetPixel(swatch.width / 2, swatch.height / 2));
        }

        /// <summary>
        /// Tests whether the swatches generated are cached and not remade.
        /// </summary>
        [Test]
        public void GeneratedSwatchAreCached()
        {
            // Arrange.
            SwatchGenerator swatchGenerator = new SwatchGenerator();
            Color targetColor = Color.magenta;
            
            // Act.
            Texture2D first = swatchGenerator.Generate(targetColor);
            Texture2D second = swatchGenerator.Generate(targetColor);
            
            // Assert.
            Assert.AreSame(first, second);
        }
    }
}

#endif