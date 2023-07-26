using System;
using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Represents a color that can be placed in a palette. Instead of being a value type like <see cref="Color"/> this is a reference type.
    /// </summary>
    [Serializable]
    public class PaletteColor
    {
        /// <summary>
        /// The color that this represents.
        /// </summary>
        [SerializeField]
        private Color _color = Color.white; 

        /// <summary>
        /// The name of this color.
        /// </summary>
        [SerializeField]
        private string _name;

        /// <summary>
        /// The unique ID of this color.
        /// </summary>
        [SerializeField]
        private string _guid = Guid.NewGuid().ToString();

        /// <summary>
        /// Called when values of this class have been updated.
        /// </summary>
        internal event Action Updated;
        
        /// <summary>
        /// The unique ID of this color.
        /// </summary>
        public Guid GUID => Guid.Parse(_guid);

        /// <summary>
        /// The color that this represents.
        /// </summary>
        public Color Color
        {
            get => _color;
            internal set
            {
                Color original = _color;
                _color = value;
                
                if(original != _color)
                    Updated?.Invoke();
            }
        }

        /// <summary>
        /// The name of this color.
        /// </summary>
        public string Name
        {
            get => _name;
            internal set
            {
                string original = _name;
                _name = value;
                
                if(original != _name)
                    Updated?.Invoke();
            }
        }

        /// <summary>
        /// Refreshes the color. Used for making sure the last value is correctly shown.
        /// </summary>
        internal void Refresh() => Updated?.Invoke();
    }
}