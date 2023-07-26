#if UNITY_EDITOR

using DTT.KeyboardRaiser.Utils;
using UnityEditor;
using UnityEngine;

namespace DTT.KeyboardRaiser.Editor.Utils
{
    /// <summary>
    /// Library for extending the functionality of the <see cref="Handles"/> class.
    /// </summary>
    internal static class HandlesLibrary
    {
        /// <summary>
        /// Draws a line with arrow caps.
        /// </summary>
        /// <param name="a">Starting point.</param>
        /// <param name="b">End point.</param>
        /// <param name="capLength">The length of the caps.</param>
        /// <param name="enableStartArrowCap">Whether to use the cap for starting point.</param>
        /// <param name="enableEndArrowCap">Whether to use the cap for ending point.</param>
        public static void DrawArrowLine(Vector3 a, Vector3 b, float capLength, bool enableStartArrowCap = true, bool enableEndArrowCap = true)
        {
            Vector3 top;
            Vector3 bottom;
            if (a.y > b.y)
                (top, bottom) = (a, b);
            else
                (bottom, top) = (a, b);
            
            Handles.DrawLine(top, bottom);
            if (enableStartArrowCap)
            {
                Handles.DrawLine(top, top - Vector3.one.Flatten() * capLength);
                Handles.DrawLine(top, top - (Vector3.left + Vector3.up) * capLength);
            }

            if (enableEndArrowCap)
            {
                Handles.DrawLine(bottom, bottom - (Vector3.right + Vector3.down) * capLength);
                Handles.DrawLine(bottom, bottom - -Vector3.one.Flatten() * capLength);
            }
        }
    }
}
#endif