using System;
using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Represents a selection made of a palette. This is used to save the selection of a palette made.
    /// </summary>
    [Serializable]
    public class PaletteSelection
    {
        /// <summary>
        /// The ID of the palette.
        /// </summary>
        [SerializeField]
        private string _paletteGuid = Guid.Empty.ToString();

        /// <summary>
        /// Called when the selection has been updated.
        /// </summary>
        public event Action Updated;

        /// <summary>
        /// The palette that was selected.
        /// </summary>
        public Palette Palette
        {
            get => PaletteDatabase.Instance.GetPalette(Guid.Parse(_paletteGuid));
            set
            {
                if (!PaletteDatabase.Instance.Contains(value.GUID))
                    throw new ArgumentException("The given palette does not exist in the database.");
                
                _paletteGuid = value.GUID.ToString();
                Updated?.Invoke();
            }
        }
    }
}