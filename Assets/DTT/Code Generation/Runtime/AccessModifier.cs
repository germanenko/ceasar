namespace DTT.CodeGeneration
{
    /// <summary>
    /// The different types of access that C# allows its members to be.
    /// </summary>
    public enum AccessModifier
    {
        /// <summary>
        /// The type or member can be accessed only by code in the same class or struct.
        /// </summary>
        PRIVATE = 0,
        
        /// <summary>
        /// The type or member can be accessed by types derived from the class that are declared within its containing assembly.
        /// </summary>
        PRIVATE_PROTECTED = 1,

        /// <summary>
        /// The type or member can be accessed only by code in the same class, or in a class that is derived from that class.
        /// </summary>
        PROTECTED = 2,
        
        /// <summary>
        /// The type or member can be accessed by any code in the same assembly, but not from another assembly. In other words, internal types or members can be accessed from code that is part of the same compilation.
        /// </summary>
        INTERNAL = 3,
        
        /// <summary>
        /// The type or member can be accessed by any code in the assembly in which it's declared, or from within a derived class in another assembly.
        /// </summary>
        PROTECTED_INTERNAL = 4,
        
        /// <summary>
        /// The type or member can be accessed by any other code in the same assembly or another assembly that references it. The accessibility level of public members of a type is controlled by the accessibility level of the type itself.
        /// </summary>
        PUBLIC = 5,
    }
}