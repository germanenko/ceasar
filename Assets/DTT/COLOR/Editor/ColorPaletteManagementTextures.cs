using System.IO;
using DTT.PublishingTools;
using DTT.Utils.Optimization;
using UnityEditor;
using UnityEngine;

namespace DTT.COLOR.Editor
{
    /// <summary>
    /// Provides textures for the color palette window.
    /// </summary>
    internal class ColorPaletteManagementTextures : LazyTexture2DCache
    {
        /// <summary>
        /// A trash bin icon.
        /// </summary>
        public Texture2D TrashBin => EditorGUIUtility.isProSkin ? TrashBinIconLight : TrashBinIconDark;
        
        /// <summary>
        /// A dark trash bin icon.
        /// </summary>
        public Texture2D TrashBinIconDark => base[nameof(TrashBinIconDark)];
        
        /// <summary>
        /// A light trash bin icon.
        /// </summary>
        public Texture2D TrashBinIconLight => base[nameof(TrashBinIconLight)];
        
        /// <summary>
        /// A palette icon.
        /// </summary>
        public Texture2D Palette => EditorGUIUtility.isProSkin ? PaletteLight : PaletteDark;
        
        /// <summary>
        /// A dark palette icon.
        /// </summary>
        public Texture2D PaletteDark => base[nameof(PaletteDark)];
        
        /// <summary>
        /// A light palette icon.
        /// </summary>
        public Texture2D PaletteLight => base[nameof(PaletteLight)];
        
        /// <summary>
        /// An arrow up icon.
        /// </summary>
        public Texture2D ArrowUp => EditorGUIUtility.isProSkin ? ArrowUpLight : ArrowUpDark;
        
        /// <summary>
        /// A dark arrow up bin icon.
        /// </summary>
        public Texture2D ArrowUpDark => base[nameof(ArrowUpDark)];
        
        /// <summary>
        /// A light arrow up icon.
        /// </summary>
        public Texture2D ArrowUpLight => base[nameof(ArrowUpLight)];
        
        /// <summary>
        /// An arrow down icon.
        /// </summary>
        public Texture2D ArrowDown => EditorGUIUtility.isProSkin ? ArrowDownLight : ArrowDownDark;
        
        /// <summary>
        /// A dark arrow down icon.
        /// </summary>
        public Texture2D ArrowDownDark => base[nameof(ArrowDownDark)];
        
        /// <summary>
        /// A light arrow down icon.
        /// </summary>
        public Texture2D ArrowDownLight => base[nameof(ArrowDownLight)];
        
        /// <summary>
        /// An edit icon.
        /// </summary>
        public Texture2D Edit => EditorGUIUtility.isProSkin ? EditLight : EditDark;
        
        /// <summary>
        /// A dark edit icon.
        /// </summary>
        public Texture2D EditDark => base[nameof(EditDark)];
        
        /// <summary>
        /// A light edit icon.
        /// </summary>
        public Texture2D EditLight => base[nameof(EditLight)];

        /// <summary>
        /// A insert icon.
        /// </summary>
        public Texture2D Insert => base[nameof(Insert)];
        
        /// <summary>
        /// A save icon.
        /// </summary>
        public Texture2D Save => base[nameof(Save)];

        /// <summary>
        /// The base path for icon storage in packages.
        /// </summary>
        private readonly string _packageBasePath = Path.Combine("Packages", "dtt.color-palette-manager", "Editor", "Icons");

        /// <summary>
        /// The base path for icon storage in assets.
        /// </summary>
        private readonly string _assetsBasePath = Path.Combine("Assets", "DTT", DTTEditorConfig.GetAssetJson("dtt.color-palette-manager").displayName, "Editor", "Icons");

        /// <summary>
        /// Create a new instance of ColorPaletteWindowTextures.
        /// </summary>
        public ColorPaletteManagementTextures()
        {
            AssetJson assetJson = DTTEditorConfig.GetAssetJson("dtt.color-palette-manager");
            string relevantPath = assetJson.assetStoreRelease ? _assetsBasePath : _packageBasePath;
            
            Add(nameof(TrashBinIconDark), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Bin Dark.png")));
            Add(nameof(TrashBinIconLight), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Bin Light.png")));
            Add(nameof(PaletteDark), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Palette Dark.png")));
            Add(nameof(PaletteLight), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Palette Light.png")));
            Add(nameof(ArrowUpDark), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Arrow Up Dark.png")));
            Add(nameof(ArrowUpLight), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Arrow Up Light.png")));
            Add(nameof(ArrowDownDark), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Arrow Down Dark.png")));
            Add(nameof(ArrowDownLight), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Arrow Down Light.png")));
            Add(nameof(EditDark), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Edit Dark.png")));
            Add(nameof(EditLight), () => AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(relevantPath, "Edit Light.png")));
            Add(nameof(Insert), () => (Texture2D)EditorGUIUtility.IconContent("CreateAddNew").image);
            Add(nameof(Save), () => (Texture2D)EditorGUIUtility.IconContent("SaveAs").image);
        }
    }
}