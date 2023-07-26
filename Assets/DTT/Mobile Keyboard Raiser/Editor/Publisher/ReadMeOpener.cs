#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Class that handles opening the editor window for the analytics package.
    /// </summary>
    internal class ReadMeOpener
    {
        /// <summary>
        /// Opens the readme for this package.
        /// </summary>
        [MenuItem("Tools/DTT/Keyboard Raiser/ReadMe")]
        private static void OpenReadMe() => DTTEditorConfig.OpenReadMe("dtt.keyboardraiser");
    }
}
#endif