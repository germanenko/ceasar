using System;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// Provides serialized types a way to to filter 
    /// in the type selector window.
    /// </summary>
    public class TypeFilterAttribute : PropertyAttribute
    {
        #region Variables
        #region Public
        /// <summary>
        /// The filter type to use.
        /// </summary>
        public readonly Type filterType;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Assigns the filter type.
        /// </summary>
        /// <param name="filterType">The filter type to use.</param>
        public TypeFilterAttribute(Type filterType) => this.filterType = filterType;
        #endregion
    }
}
