using System.Collections;
using DTT.COLOR.ColorMatching;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DTT.COLOR.Tests
{
    /// <summary>
    /// Tests the <see cref="ColorMatcher"/> object.
    /// </summary>
    public class ColorMatcherTests
    {
        /// <summary>
        /// Testable palette.
        /// </summary>
        private Palette _palette;
        
        /// <summary>
        /// Testable palette color.
        /// </summary>
        private PaletteColor _paletteColor;
        
        /// <summary>
        /// Sets up the testable palette and color.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _palette = new Palette() { Name = "Testing Palette" };
            _paletteColor = new PaletteColor() { Name = "Testing Color", Color = Color.magenta};
            
            _palette.Add(_paletteColor);
            PaletteDatabase.Instance.Add(_palette);
        }

        /// <summary>
        /// Clears the testable palette.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown() => PaletteDatabase.Instance.Remove(_palette);

        /// <summary>
        /// Expects the color of the testable palette color to be applied to the graphic.
        /// </summary>
        [UnityTest]
        public IEnumerator ColorMatcherAppliesToGraphic()
        {
            // Arrange.
            GameObject go = new GameObject();
            Image image = go.AddComponent<Image>();
            ColorMatcher colorMatcher = go.AddComponent<ColorMatcher>();
            colorMatcher.Selection.Palette = _palette;
            colorMatcher.Selection.PaletteColor = _paletteColor;
            
            // Act.
            // Wait for color to be applied.
            yield return null;
            
            // Assert.
            Assert.AreEqual(_paletteColor.Color, image.color);
            
            // Clean up.
            Object.Destroy(go);
        }
        
        /// <summary>
        /// Expects the color of the testable palette color to be applied to the sprite renderer.
        /// </summary>
        [UnityTest]
        public IEnumerator ColorMatcherAppliesToSpriteRenderer()
        {
            // Arrange.
            GameObject go = new GameObject();
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            ColorMatcher colorMatcher = go.AddComponent<ColorMatcher>();
            colorMatcher.Selection.Palette = _palette;
            colorMatcher.Selection.PaletteColor = _paletteColor;
            
            // Act.
            // Wait for color to be applied.
            yield return null;
            
            // Assert.
            Assert.AreEqual(_paletteColor.Color, spriteRenderer.color);
            
            // Clean up.
            Object.Destroy(go);
        }
        
        /// <summary>
        /// Expects the color of the testable palette color to be applied to the sprite renderer.
        /// </summary>
        [UnityTest]
        public IEnumerator ColorMatcherAppliesToCamera()
        {
            // Arrange.
            GameObject go = new GameObject();
            Camera camera = go.AddComponent<Camera>();
            ColorMatcher colorMatcher = go.AddComponent<ColorMatcher>();
            colorMatcher.Selection.Palette = _palette;
            colorMatcher.Selection.PaletteColor = _paletteColor;
            
            // Act.
            // Wait for color to be applied.
            yield return null;
            
            // Assert.
            Assert.AreEqual(_paletteColor.Color, camera.backgroundColor);
            
            // Clean up.
            Object.Destroy(go);
        }
    }
}