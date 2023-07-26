using System;
using NUnit.Framework;
using UnityEngine;

namespace DTT.COLOR.Tests
{
    /// <summary>
    /// Tests the database operations for the palette database.
    /// </summary>
    public class PaletteDatabaseTests
    {
        /// <summary>
        /// Expects an instance to always be available.
        /// </summary>
        [Test]
        public void InstanceAvailable()
        {
            // Act.
            PaletteDatabase database = PaletteDatabase.Instance;

            // Assert.
            Assert.IsNotNull(database);
        }

        /// <summary>
        /// Expects to retrieve the same color instance as was added.
        /// </summary>
        [Test]
        public void GetColorRetrievesColor()
        {
            // Arrange.
            Palette palette = new Palette() { Name = "Test Palette" };
            PaletteColor paletteColor = new PaletteColor() { Name = "Test Color", Color = Color.magenta};
            
            palette.Add(paletteColor);
            PaletteDatabase.Instance.Add(palette);
            
            // Act.
            PaletteColor actual = PaletteDatabase.Instance.GetColor(palette.GUID, paletteColor.GUID);
            
            // Assert.
            Assert.AreSame(paletteColor, actual);
            
            // Clean up.
            PaletteDatabase.Instance.Remove(palette);
        }

        /// <summary>
        /// Expects to retrieve the same palette instance as was added.
        /// </summary>
        [Test]
        public void GetPaletteRetrievesPalette()
        {
            // Arrange.
            Palette palette = new Palette() { Name = "Test Palette" };
            
            PaletteDatabase.Instance.Add(palette);
            
            // Act.
            Palette actual = PaletteDatabase.Instance.GetPalette(palette.GUID);
            
            // Assert.
            Assert.AreSame(palette, actual);
            
            // Clean up.
            PaletteDatabase.Instance.Remove(palette);
        }

        /// <summary>
        /// Retrieving a color that doesn't exist should return null.
        /// </summary>
        [Test]
        public void GetColorDoesntExistReturnsNull()
        {
            // Act.
            PaletteColor actual = PaletteDatabase.Instance.GetColor(Guid.NewGuid(), Guid.NewGuid());
            
            // Assert.
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Retrieving a palette that doesn't exist should return null.
        /// </summary>
        [Test]
        public void GetPaletteDoesntExistReturnsNull()
        {
            // Act.
            Palette actual = PaletteDatabase.Instance.GetPalette(Guid.NewGuid());
            
            // Assert.
            Assert.IsNull(actual);
        }
    }
}