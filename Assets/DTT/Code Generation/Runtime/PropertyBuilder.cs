using System;
using System.Linq;
using System.Text;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Allows for building a C# source property.
    /// </summary>
    public class PropertyBuilder : IPropertyBuilder
    {
        /// <summary>
        /// The type of the property.
        /// </summary>
        private Type _type;
        
        /// <summary>
        /// The name of the property.
        /// </summary>
        private string _name;
        
        /// <summary>
        /// Whether the property is marked static.
        /// </summary>
        private bool _isStatic;
        
        /// <summary>
        /// The access modifier of the property.
        /// </summary>
        private AccessModifier _access;
        
        /// <summary>
        /// The get value of the property.
        /// </summary>
        private string _getValue = string.Empty;

        /// <summary>
        /// Creates a new Property Builder instance.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        public PropertyBuilder(string name, Type type)
        {
            SetName(name);
            SetType(type);
        }

        /// <summary>
        /// Sets the type of the property.
        /// </summary>
        /// <param name="type">The type it should be declared with.</param>
        /// <returns>The builder instance.</returns>
        public IPropertyBuilder SetType(Type type)
        {
            _type = type;
            return this;
        }

        /// <summary>
        /// Sets the name of the property.
        /// </summary>
        /// <param name="name">The name the property should be.</param>
        /// <returns>The builder instance.</returns>
        public IPropertyBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        /// <summary>
        /// Sets whether the property should be marked static.
        /// </summary>
        /// <param name="isStatic">Whether the property should be marked static.</param>
        /// <returns>The builder instance.</returns>
        public IPropertyBuilder SetStatic(bool isStatic)
        {
            _isStatic = isStatic;
            return this;
        }

        /// <summary>
        /// Sets the access modifier of the property.
        /// </summary>
        /// <param name="access">The access it should have.</param>
        /// <returns>The builder instance.</returns>
        public IPropertyBuilder SetAccess(AccessModifier access)
        {
            _access = access;
            return this;
        }

        /// <summary>
        /// Sets the return value of the property. This has to be a single lined statement that can be returned.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <returns>The builder instance.</returns>
        public IPropertyBuilder SetGetValue(string value)
        {
            _getValue = value;
            return this;
        }

        /// <summary>
        /// Exports the object to a stringified version of C# source code.
        /// </summary>
        /// <returns>A stringified version of C# source code.</returns>
        public string Export()
        {
            if (string.IsNullOrEmpty(_getValue)) throw new InvalidOperationException("No return value was added while exporting. This will guaranteed create compile errors during compilation.");
            
            StringBuilder builder = new StringBuilder();
            string typeName = string.Empty;
            if (_type != null)
                typeName = _type.FullName;

            builder.Append(_access.MapToSource());
            builder.Append(' ');
            if (_isStatic)
            {
                builder.AppendFormat("{0}", SourceKeywords.STATIC);
                builder.Append(' ');
            }
            builder.Append(typeName);
            builder.Append(' ');
            builder.Append(_name);
            
            builder.AppendLine("{");
            builder.AppendLine("\tget");
            builder.AppendLine("\t{");
            builder.AppendFormat("\t\treturn {0}{1}{2}", _getValue, _getValue.LastOrDefault() == ';' ? string.Empty : ";", Environment.NewLine);
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}