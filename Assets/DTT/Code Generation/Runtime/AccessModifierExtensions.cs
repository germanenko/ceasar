using System;

namespace DTT.CodeGeneration
{
    /// <summary>
    /// Provides extension methods for the <see cref="AccessModifier"/> enum.
    /// </summary>
    public static class AccessModifierExtensions
    {
        /// <summary>
        /// Returns the C# source representation of the enum value.
        /// </summary>
        /// <param name="access">The enum value to retrieve the keyword for.</param>
        /// <returns>The keyword value for the passed enum.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an unsupported access modifier is used.</exception>
        public static string MapToSource(this AccessModifier access)
        {
            switch (access)
            {
                case AccessModifier.PRIVATE:
                    return SourceKeywords.PRIVATE;
                case AccessModifier.PRIVATE_PROTECTED:
                    return $"{SourceKeywords.PRIVATE} {SourceKeywords.PROTECTED}";
                case AccessModifier.PROTECTED:
                    return SourceKeywords.PROTECTED;
                case AccessModifier.INTERNAL:
                    return SourceKeywords.INTERNAL;
                case AccessModifier.PROTECTED_INTERNAL:
                    return $"{SourceKeywords.PROTECTED} {SourceKeywords.INTERNAL}";
                case AccessModifier.PUBLIC:
                    return SourceKeywords.PUBLIC;
                default:
                    throw new ArgumentOutOfRangeException(nameof(access), access, null);
            }
        }
    }
}