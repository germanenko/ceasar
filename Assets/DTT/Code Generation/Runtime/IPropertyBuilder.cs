using System;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Defines an interface building C# source properties.
    /// </summary>
    public interface IPropertyBuilder : IExportable
    {
        /// <summary>
        /// Sets the type of the property.
        /// </summary>
        /// <param name="type">The type to set the property to.</param>
        /// <returns>The builder instance.</returns>
        IPropertyBuilder SetType(Type type);
        
        /// <summary>
        /// Sets the name of the property.
        /// </summary>
        /// <param name="name">The name to set the property to.</param>
        /// <returns>The builder instance.</returns>
        IPropertyBuilder SetName(string name);
        
        /// <summary>
        /// Able to mark to property to static.
        /// </summary>
        /// <param name="isStatic">Whether it is static.</param>
        /// <returns>The builder instance.</returns>
        IPropertyBuilder SetStatic(bool isStatic);
        
        /// <summary>
        /// Sets the access modifier of the property to the provided option.
        /// </summary>
        /// <param name="access">The access the property should have.</param>
        /// <returns>The builder instance.</returns>
        IPropertyBuilder SetAccess(AccessModifier access);
        
        /// <summary>
        /// Sets the get value of the property.
        /// </summary>
        /// <param name="value">The value that should be used as return value.</param>
        /// <returns>The builder instance.</returns>
        IPropertyBuilder SetGetValue(string value);
    }
}