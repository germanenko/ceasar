using UnityEngine;

namespace DTT.KeyboardRaiser.Utils
{
    /// <summary>
    /// Provides additional functionalities to <see cref="Vector3"/>.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Flattens the Z coordinate of a vector to zero.
        /// </summary>
        /// <param name="vector">The vector to flatten.</param>
        /// <returns>The flattened vector.</returns>
        public static Vector3 Flatten(this Vector3 vector) => new Vector3(vector.x, vector.y, 0);
    }
}