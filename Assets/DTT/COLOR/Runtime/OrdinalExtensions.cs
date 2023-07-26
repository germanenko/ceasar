using System;

namespace DTT.COLOR
{
    /// <summary>
    /// Extension methods for the <see cref="Ordinal"/> enum.
    /// </summary>
    public static class OrdinalExtensions
    {
        /// <summary>
        /// Maps a display name to the enum member.
        /// </summary>
        /// <param name="ordinal">The enum member.</param>
        /// <returns>The display name.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if enum is not supported.</exception>
        public static string MapReadableName(this Ordinal ordinal)
        {
            switch (ordinal)
            {
                case Ordinal.PRIMARY:
                    return "Primary";
                case Ordinal.SECONDARY:
                    return "Secondary";
                case Ordinal.TERTIARY:
                    return "Tertiary";
                case Ordinal.QUATERNARY:
                    return "Quaternary";
                case Ordinal.QUINARY:
                    return "Quinary";
                case Ordinal.SENARY:
                    return "Senary";
                case Ordinal.SEPTENARY:
                    return "Septenary";
                case Ordinal.OCTONARY:
                    return "Octonary";
                case Ordinal.NONARY:
                    return "Nonary";
                case Ordinal.DENARY:
                    return "Denary";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ordinal), ordinal, $"No readable name for {ordinal}");
            }
        }
    }
}