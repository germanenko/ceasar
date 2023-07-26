using System;
using System.Collections.Generic;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Provides additional utility to a string instance.
    /// </summary>
    internal static class StringUtility
    {
        /// <summary>
        /// Indents all the new lines of a string.
        /// </summary>
        /// <param name="value">The string to indent.</param>
        /// <returns>An indented string.</returns>
        public static string Indent(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            
            // Places an indent at the start of the string.
            value = value.Insert(0, "\t");
            
            // Retrieves all the indices of the string that is a new line.
            // It skips the last character since no indent should be placed on an empty new line.
            List<int> lineIndices = new List<int>();
            for (int j = 0; j < value.Length - 1; j++)
                if (value[j] == '\n')
                    lineIndices.Add(j);

            // Inserts all the indents on all the new lines.
            // Using reversed loop so the added character / length don't interfere with insertion.
            for (int j = lineIndices.Count - 1; j >= 0; j--)
                value = value.Insert(lineIndices[j] + 1, "\t");

            return value;
        }
    }
}