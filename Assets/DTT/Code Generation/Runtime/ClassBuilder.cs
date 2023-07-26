using System;
using System.Collections.Generic;
using System.Text;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Used for building source file C# classes.
    /// </summary>
    public class ClassBuilder : IClassBuilder
    {
        /// <summary>
        /// All the current property builders in the class.
        /// </summary>
        private readonly List<IPropertyBuilder> _propertyBuilders = new List<IPropertyBuilder>();
        
        /// <summary>
        /// The name used for this class.
        /// </summary>
        private string _className;
        
        /// <summary>
        /// Whether this class is marked static.
        /// </summary>
        private bool _isStatic;
        
        /// <summary>
        /// Whether this class is marked readonly.
        /// </summary>
        private bool _isReadonly;
        
        /// <summary>
        /// The access modifier of this class.
        /// </summary>
        private AccessModifier _access;

        /// <summary>
        /// Creates an instance of the class builder.
        /// </summary>
        /// <param name="name">The name of your class.</param>
        public ClassBuilder(string name)
        {
            SetName(name);
        }

        /// <summary>
        /// Adds a property to this class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <returns>The property builder used to define it.</returns>
        public IPropertyBuilder AddProperty(string name, Type type)
        {
            IPropertyBuilder propertyBuilder = new PropertyBuilder(name, type);

            _propertyBuilders.Add(propertyBuilder);
            return propertyBuilder;
        }

        /// <summary>
        /// Sets the name of this class.
        /// </summary>
        /// <param name="className">The name to set this class to.</param>
        /// <returns>The builder instance.</returns>
        public IClassBuilder SetName(string className)
        {
            _className = className;
            return this;
        }

        /// <summary>
        /// Able to set this class to static.
        /// </summary>
        /// <param name="isStatic">Whether it is static.</param>
        /// <returns>The builder instance.</returns>
        public IClassBuilder SetStatic(bool isStatic)
        {
            _isStatic = isStatic;
            return this;
        }

        /// <summary>
        /// Able to set this class to readonly.
        /// </summary>
        /// <param name="isReadonly">Whether it is readonly.</param>
        /// <returns>The builder instance.</returns>
        public IClassBuilder SetReadonly(bool isReadonly)
        {
            _isReadonly = isReadonly;
            return this;
        }

        /// <summary>
        /// Able to set the access modifier of this class.
        /// </summary>
        /// <param name="access">The access of this class.</param>
        /// <returns>The builder instance.</returns>
        public IClassBuilder SetAccess(AccessModifier access)
        {
            _access = access;
            return this;
        }
        
        /// <summary>
        /// Exports the object to a stringified version of C# source code.
        /// </summary>
        /// <returns>A stringified version of C# source code.</returns>
        public string Export()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0} ", _access.MapToSource());
            if (_isStatic)
                stringBuilder.AppendFormat("{0} ", SourceKeywords.STATIC);
            if (_isReadonly)
                stringBuilder.AppendFormat("{0} ", SourceKeywords.READONLY);

            stringBuilder.AppendFormat("{0} {1}{2}{{{3}", SourceKeywords.CLASS, _className, Environment.NewLine, Environment.NewLine);
            
            StringBuilder properties = new StringBuilder();
            for (int i = 0; i < _propertyBuilders.Count; i++)
            {
                properties.Append(_propertyBuilders[i].Export());
                
                if (i != _propertyBuilders.Count - 1)
                    properties.AppendLine();
            }

            if(_propertyBuilders.Count > 0)
                stringBuilder.Append(properties.ToString().Indent());
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }
    }
}