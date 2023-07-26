namespace DTT.CodeGeneration
{
    public interface IExportable
    {
        /// <summary>
        /// Exports the object to a stringified version of C# source code.
        /// </summary>
        /// <returns>A stringified version of C# source code.</returns>
        string Export();
    }
}