using System;
using System.Collections.Generic;
using System.Text;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Allows for building a C# source script.
    /// </summary>
    public class ScriptBuilder : IScriptBuilder
    {
        /// <summary>
        /// The string builder for building the namespaces.
        /// </summary>
        private readonly StringBuilder _namespaceStringBuilder  = new StringBuilder();
        
        /// <summary>
        /// All the builders that build classes in the script.
        /// </summary>
        private readonly List<IClassBuilder> _classBuilders  = new List<IClassBuilder>();
        
        /// <summary>
        /// The name of the namespace.
        /// </summary>
        private string _namespace;
        
        /// <summary>
        /// The disclaimers placed at the top of the file.
        /// </summary>
        private string[] _disclaimer = Array.Empty<string>();
        
        /// <summary>
        /// Adds a class to the script builder.
        /// </summary>
        /// <param name="name">The name of your class.</param>
        /// <returns>The class builder instance.</returns>
        public IClassBuilder AddClass(string name)
        {
            IClassBuilder classBuilder = new ClassBuilder(name);
            _classBuilders.Add(classBuilder);
            return classBuilder;
        }

        /// <summary>
        /// Imports a namespace in the script.
        /// </summary>
        /// <param name="namespaceName">The name of the namespace to import.</param>
        /// <returns>The builder instance.</returns>
        public IScriptBuilder ImportNamespace(string namespaceName)
        {
            _namespaceStringBuilder.AppendFormat("using {0};{1}", namespaceName, Environment.NewLine);
            return this;
        }

        /// <summary>
        /// Sets the disclaimers at the of the file. Every new index is placed on a new line.
        /// </summary>
        /// <param name="disclaimer">The disclaimer placed at the top of the file.</param>
        /// <returns>The builder instance.</returns>
        public IScriptBuilder SetDisclaimer(params string[] disclaimer)
        {
            // Redefine the array if the lengths don't match.
            if(disclaimer.Length != _disclaimer.Length)
                _disclaimer = new string[disclaimer.Length];
            
            // Copy the contents.
            disclaimer.CopyTo(_disclaimer, 0);
            return this;
        }

        /// <summary>
        /// Sets the namespace the classes will reside in.
        /// </summary>
        /// <param name="namespace">The name to set the namespace to.</param>
        /// <returns>The builder instance.</returns>
        public IScriptBuilder SetNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        /// <summary>
        /// Exports the object to a stringified version of C# source code.
        /// </summary>
        /// <returns>A stringified version of C# source code.</returns>
        public string Export()
        {
            StringBuilder export = new StringBuilder();
            // Places the disclaimers.
            for (int i = 0; i < _disclaimer.Length; i++)
                export.AppendLine($"// {_disclaimer[i]}");
            
            // Places the using statements for the namespaces.
            export.AppendLine(_namespaceStringBuilder.ToString());

            // Starts the namespace.
            if (!string.IsNullOrEmpty(_namespace)) 
                export.AppendFormat("namespace {0}{1}{{{2}", _namespace, Environment.NewLine, Environment.NewLine);

            for (int i = 0; i < _classBuilders.Count; i++)
            {
                string classExport = _classBuilders[i].Export();

                // Indents the class if a namespace is defined.
                if (!string.IsNullOrEmpty(_namespace)) 
                    classExport = classExport.Indent();

                export.Append(classExport);
                
                // Only add new lines if it's not the last.
                if (i != _classBuilders.Count - 1)
                    export.AppendLine();
            }

            // Ends the namespace.
            if (!string.IsNullOrEmpty(_namespace))
                export.AppendLine("}");

            return export.ToString();
        }
    }
}