using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Saves all the palettes and colors.
    /// </summary>
    public class PaletteDatabase : ScriptableObject
    {
        /// <summary>
        /// The main instance of the palette database.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if the Palette Database can't be found.</exception>
        public static PaletteDatabase Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<PaletteDatabase>("Palette Database");

                if (_instance == null)
                    throw new FileNotFoundException("Expected to find Palette Database.asset in 'Assets/DTT/COLOR/Resources/'");
                
                return _instance;
            }
        }

        /// <summary>
        /// The main instance.
        /// </summary>
        private static PaletteDatabase _instance;
        
        /// <summary>
        /// All the composed palettes.
        /// </summary>
        [SerializeField]
        private List<Palette> _palettes = new List<Palette>();

        /// <summary>
        /// All the palettes in the database.
        /// </summary>
        public ReadOnlyCollection<Palette> Palettes => _palettes.AsReadOnly();

        /// <summary>
        /// Retrieves the color by GUID.
        /// </summary>
        /// <param name="paletteGuid">The ID of the palette.</param>
        /// <param name="paletteColorGuid">The ID of the color.</param>
        /// <returns>The palette color or null if it can't be found.</returns>
        public PaletteColor GetColor(Guid paletteGuid, Guid paletteColorGuid)
        {
            if (paletteColorGuid == Guid.Empty)
                return new PaletteColor{ Color = Color.white, Name = "No color" };

            return _palettes.FirstOrDefault(palette => palette.GUID == paletteGuid)?.FirstOrDefault(color => color.GUID == paletteColorGuid);
        }

        /// <summary>
        /// Retrieves the palette by GUID.
        /// </summary>
        /// <param name="paletteGuid">The ID of the palette.</param>
        /// <returns>The palette or null if it can't be found.</returns>
        public Palette GetPalette(Guid paletteGuid)
        {
            if (paletteGuid == Guid.Empty)
                return new Palette { Name = "No Palette" };

            return _palettes.FirstOrDefault(palette => palette.GUID == paletteGuid);
        }

        /// <summary>
        /// Adds the palette to the database.
        /// </summary>
        /// <param name="palette">The palette to add.</param>
        internal void Add(Palette palette) => _palettes.Add(palette);

        /// <summary>
        /// Removes the palette from the database.
        /// </summary>
        /// <param name="palette">The palette to remove.</param>
        internal void Remove(Palette palette) => _palettes.Remove(palette);

        /// <summary>
        /// Checks whether the given GUID is a palette that exists.
        /// </summary>
        /// <param name="paletteGuid">The Guid to check on.</param>
        /// <returns>Whether this palette exists.</returns>
        public bool Contains(Guid paletteGuid) => _palettes.Any(palette => palette.GUID.CompareTo(paletteGuid) == 0);
    }
}