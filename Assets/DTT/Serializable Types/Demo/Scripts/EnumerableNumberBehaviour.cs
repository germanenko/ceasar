using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Serializables
{
    /// <summary>
    /// An enumerable number behaviour.
    /// </summary>
    public class EnumerableNumberBehaviour : MonoBehaviour, IEnumerable<int>
    {
        /// <summary>
        /// A list of numbers this behaviour holds.
        /// </summary>
        [SerializeField]
        private List<int> _numbers = new List<int>();

        /// <summary>
        /// Returns the enumerator for iterating of the numbers this behaviour holds.
        /// </summary>
        /// <returns>The enumerator of the numbers.</returns>
        public IEnumerator<int> GetEnumerator() => _numbers.GetEnumerator();

        /// <summary>
        /// Returns the enumerator for iterating of the numbers this behaviour holds.
        /// </summary>
        /// <returns>The enumerator of the numbers.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _numbers.GetEnumerator();
    }
}
