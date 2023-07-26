using System;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Represents the behaviour of all the operations that can be used to build a class.
    /// </summary>
    public interface IClassBuilder : IExportable
    {
        /// <summary>
        /// Should add a member property to the class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <returns>The property builder that is responsible to build this property.</returns>
        IPropertyBuilder AddProperty(string name, Type type);
        
        /// <summary>
        /// Sets the name of the class.
        /// </summary>
        /// <param name="className">What to call the class.</param>
        /// <returns>The builder instance.</returns>
        IClassBuilder SetName(string className);
        
        /// <summary>
        /// Sets the class to be static.
        /// </summary>
        /// <param name="isStatic">Whether it should be static.</param>
        /// <returns>The builder instance.</returns>
        IClassBuilder SetStatic(bool isStatic);
        
        /// <summary>
        /// Sets the class to be readonly.
        /// </summary>
        /// <param name="isReadonly">Whether it should be readonly.</param>
        /// <returns>The builder instance.</returns>
        IClassBuilder SetReadonly(bool isReadonly);
        
        /// <summary>
        /// Sets the access modifier of the class.
        /// </summary>
        /// <param name="access">The modifier to set.</param>
        /// <returns>The builder instance.</returns>
        IClassBuilder SetAccess(AccessModifier access);
    }
}