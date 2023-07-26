using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Represents a color palette by composing colors.
    /// </summary>
    [Serializable]
    public class Palette : IReadOnlyCollection<PaletteColor>
    {
        /// <summary>
        /// The composed colors.
        /// </summary>
        [SerializeField]
        private List<PaletteColor> _colors;

        /// <summary>
        /// The name of the palette.
        /// </summary>
        [SerializeField]
        private string _name;

        /// <summary>
        /// The unique ID of this palette.
        /// </summary>
        [SerializeField]
        private string _guid = Guid.NewGuid().ToString();

        /// <summary>
        /// The unique ID of this palette.
        /// </summary>
        public Guid GUID => Guid.Parse(_guid);
        
        /// <summary>
        /// The name of this palette.
        /// </summary>
        public string Name
        {
            get => _name;
            internal set => _name = value;
        }

        /// <summary>
        /// The amount of colors in this palette.
        /// </summary>
        public int Count => _colors.Count;
        
        /// <summary>
        /// Retrieves a color by index.
        /// </summary>
        /// <param name="index">The index of the color.</param>
        public PaletteColor this[int index]
        {
            get => _colors[index];
            internal set => _colors[index] = value;
        }
        
        /// <summary>
        /// Retrieves a color by index.
        /// </summary>
        /// <param name="ordinal">The ordinal of the color. If doesn't exist, returns the last color.</param>
        public PaletteColor this[Ordinal ordinal]
        {
            get
            {
                int index = (int) ordinal;
                if (_colors.Count <= index)
                    return _colors.Count == 0 ? null : _colors.Last();
                return _colors[index];
            }
            internal set => _colors[(int)ordinal] = value;
        }

        /// <summary>
        /// Creates a new palette instance.
        /// </summary>
        public Palette() => _colors = new List<PaletteColor>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<PaletteColor> GetEnumerator() => _colors.GetEnumerator();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds a color to the palette.
        /// </summary>
        /// <param name="item">The color to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the color is null.</exception>
        internal void Add(PaletteColor item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            _colors.Add(item);
        }

        /// <summary>
        /// Clears all the colors.
        /// </summary>
        internal void Clear() => _colors.Clear();

        /// <summary>
        /// Whether the palette contains the given color.
        /// </summary>
        /// <param name="item">Whether the color is in this palette.</param>
        /// <returns>Whether the color is in this palette.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the color is null.</exception>
        public bool Contains(PaletteColor item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            return _colors.Contains(item);
        }

        /// <summary>
        /// Whether the palette contains the given color.
        /// </summary>
        /// <param name="item">Whether the color GUID is in this palette.</param>
        /// <returns>Whether the color GUID is in this palette.</returns>
        public bool Contains(Guid item) => _colors.Any(color => color.GUID.CompareTo(item) == 0);

        /// <summary>
        /// Removes the color from the palette.
        /// </summary>
        /// <param name="item">The color to remove.</param>
        /// <returns>Whether the removal was successful.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the color is null.</exception>
        internal bool Remove(PaletteColor item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            return _colors.Remove(item);
        }
    }
}