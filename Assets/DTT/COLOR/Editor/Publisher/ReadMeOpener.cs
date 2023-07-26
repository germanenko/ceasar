#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Class that handles opening the editor window for the COLOR package.
    /// </summary>
    internal static class ReadMeOpener
    {
        /// <summary>
        /// Opens the readme for this package.
        /// </summary>
        [MenuItem("Tools/DTT/COLOR/ReadMe")]
        private static void OpenReadMe() => DTTEditorConfig.OpenReadMe("dtt.color-palette-management");
    }
}
#endif