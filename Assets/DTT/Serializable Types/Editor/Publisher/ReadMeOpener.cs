#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

/// <summary>
/// Opens the readme for this package.
/// </summary>
internal class ReadMeOpener
{
    [MenuItem("Tools/DTT/SerializableTypes/ReadMe")]
    private static void OpenReadMe() => DTTEditorConfig.OpenReadMe("dtt.serializabletypes");
}

#endif
