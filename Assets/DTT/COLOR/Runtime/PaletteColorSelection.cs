using System;
using System.Linq;
using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Represents a selection made of a color. This is used to save the selection of a color made.
    /// </summary>
    [Serializable]
    public class PaletteColorSelection
    {
        /// <summary>
        /// The ID of the palette.
        /// </summary>
        [SerializeField]
        private string _paletteGuid = Guid.Empty.ToString();
        
        /// <summary>
        /// The ID of the color.
        /// </summary>
        [SerializeField]
        private string _paletteColorGuid = Guid.Empty.ToString();

        /// <summary>
        /// Called when the selection has been updated.
        /// </summary>
        internal event Action Updated;

        /// <summary>
        /// The color that was selected.
        /// </summary>
        public PaletteColor PaletteColor
        {
            get => PaletteDatabase.Instance.GetColor(Guid.Parse(_paletteGuid), Guid.Parse(_paletteColorGuid));
            set
            {
                if (!PaletteDatabase.Instance.Contains(Guid.Parse(_paletteGuid)))
                    throw new ArgumentException("The given palette does not exist in the database.");
                if (!PaletteDatabase.Instance.GetPalette(Guid.Parse(_paletteGuid)).Contains(value))
                    throw new ArgumentException("The given color does not exist in the palette.");
                
                string original = _paletteColorGuid;
                _paletteColorGuid = value.GUID.ToString();
                
                // Only invokes when the color changes, since changing the palette means changing the color.
                if (original != _paletteColorGuid)
                {
                    // Safeguard try-catch for if the action invocation causes an exception. 
                    try
                    {
                        Updated?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

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
                _paletteColorGuid = Guid.Empty.ToString();
                try
                {
                    Updated?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}