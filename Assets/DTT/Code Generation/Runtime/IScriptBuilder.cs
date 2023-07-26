namespace DTT.CodeGeneration
{
    /// <summary>
    /// Defines an interface for building a C# source code script.
    /// </summary>
    public interface IScriptBuilder : IExportable
    {
        /// <summary>
        /// Adds a class to the script.
        /// </summary>
        /// <param name="name">The name of your class.</param>
        /// <returns>The class builder instance.</returns>
        IClassBuilder AddClass(string name);
        
        /// <summary>
        /// Imports a namespace into the script.
        /// </summary>
        /// <param name="namespaceName">The namespace to import.</param>
        /// <returns>The builder instance.</returns>
        IScriptBuilder ImportNamespace(string namespaceName);
        
        /// <summary>
        /// Sets a disclaimer on the file.
        /// </summary>
        /// <param name="disclaimer">The disclaimer messages.</param>
        /// <returns>The builder instance.</returns>
        IScriptBuilder SetDisclaimer(params string[] disclaimer);
        
        /// <summary>
        /// Sets the namespace everything in the script will reside in.
        /// </summary>
        /// <param name="namespace">The namespace that name.</param>
        /// <returns>The builder instance.</returns>
        IScriptBuilder SetNamespace(string @namespace);
    }
}