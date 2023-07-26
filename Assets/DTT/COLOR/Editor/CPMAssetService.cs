using System.IO;
using DTT.PublishingTools;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Handles the assets for the CPM package.
    /// </summary>
    internal static class CPMAssetService
    {
        /// <summary>
        /// Checks if a database exists if not it creates the default one in the resources folder.
        /// </summary>
        [DidReloadScripts]
        private static void DidReloadScripts()
        {
            string[] databaseGuids = AssetDatabase.FindAssets("t:PaletteDatabase");
            if (databaseGuids.Length != 0)
                return;
            
            string source = Path.Combine(DTTEditorConfig.GetFullContentFolderPath(DTTEditorConfig.GetAssetJson("dtt.color-palette-manager")), "Copiable Assets", "Palette Database");
            
            string destinationFolder = Path.Combine(Application.dataPath, "DTT", "COLOR", "Resources");
            string destination = Path.Combine(destinationFolder, "Palette Database.asset");
             
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder); 
            File.Copy(source, destination);
        }
    }
}