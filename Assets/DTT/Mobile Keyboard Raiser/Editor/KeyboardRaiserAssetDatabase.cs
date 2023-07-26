#if UNITY_EDITOR

using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using System.IO;
using UnityEditor;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Handles getting and creating the <see cref="KeyboardRaiserConfig"/>.
    /// </summary>
    internal static class KeyboardRaiserAssetDatabase
    {
        /// <summary>
        /// The package name of this package as set in the package.json file.
        /// </summary>
        public static string PACKAGE_NAME = "dtt.keyboardraiser";

        /// <summary>
        /// Reference to the asset json file of this asset.
        /// </summary>
        private static AssetJson _assetJson;

        /// <summary>
        /// Gets the correct directory path for the <see cref="KeyboardRaiserConfig"/> file.
        /// </summary>
        public static string AssetDirectoryPath =>
            Path.Combine(
                "Assets",
                "DTT",
                _assetJson.displayName,
                "Resources"
            );

        /// <summary>
        /// Reference to the <see cref="KeyboardRaiserConfig"/> of this project.
        /// </summary>
        public static KeyboardRaiserConfig Config { get; private set; }

        /// <summary>
        /// Gets the property cache for the <see cref="KeyboardRaiserConfig"/>.
        /// </summary>
        public static KeyboardRaiserConfigPropertyCache ConfigPropertyCache { get; private set; }

        /// <summary>
        /// Initially gets the asset json data.
        /// </summary>
        static KeyboardRaiserAssetDatabase() => _assetJson = DTTEditorConfig.GetAssetJson(PACKAGE_NAME);

        /// <summary>
        /// Initialized the <see cref="KeyboardRaiserConfig"/> file.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void LoadConfig() => GetOrCreateKeyboardRaiserConfigAsset();

        /// <summary>
        /// Creates the <see cref="KeyboardRaiserConfig"/> object inside the resources folder. 
        /// Will return the asset reference if it already exists.
        /// </summary>
        /// <returns>The found/created <see cref="KeyboardRaiserConfig"/> asset.</returns>
        public static KeyboardRaiserConfig GetOrCreateKeyboardRaiserConfigAsset()
        {
            // Make sure the default asset path is created.
            string directoryPath = AssetDirectoryPath;

            Directory.CreateDirectory(directoryPath);

            // Create or find the config asset and save it.
            string assetPath = Path.Combine(directoryPath, nameof(KeyboardRaiserConfig) + ".asset");
            Config = AssetDatabaseUtility.GetOrCreateScriptableObjectAsset<KeyboardRaiserConfig>(assetPath);
            ConfigPropertyCache = new KeyboardRaiserConfigPropertyCache(new SerializedObject(Config));

            // Save the asset database changes.
            AssetDatabase.SaveAssets();

            return Config;
        }
    }
}

#endif