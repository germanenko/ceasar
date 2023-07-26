using UnityEngine;

namespace DTT.COLOR
{
    /// <summary>
    /// Ordinalities used for indexing color palettes.
    /// </summary>
    public enum Ordinal
    {
        [InspectorName("Primary")]
        PRIMARY = 0,
        
        [InspectorName("Secondary")]
        SECONDARY = 1,
        
        [InspectorName("Tertiary")]
        TERTIARY = 2,
        
        [InspectorName("Quaternary")]
        QUATERNARY = 3,
        
        [InspectorName("Quinary")]
        QUINARY = 4,
        
        [InspectorName("Senary")]
        SENARY = 5,
        
        [InspectorName("Septenary")]
        SEPTENARY = 6,
        
        [InspectorName("Octonary")]
        OCTONARY = 7,
        
        [InspectorName("Nonary")]
        NONARY = 8,
        
        [InspectorName("Denary")]
        DENARY = 9
    }
}