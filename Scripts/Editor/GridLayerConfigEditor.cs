using UnityEditor;
using UnityEngine;
using System.IO;

namespace TinyWalnutGames.GridLayerEditor
{
    /// <summary>
    /// Editor utility for creating GridLayerConfig assets via the Unity menu.
    /// </summary>
    public static class GridLayerConfigEditor
    {
        /// <summary>
        /// Creates a new GridLayerConfig asset in the selected folder or in Assets.
        /// </summary>
        [MenuItem("Assets/Create/Tiny Walnut Games/Grid Layer Config", priority = 1)]
        public static void CreateGridLayerConfig()
        {
            // Get the path of the selected object in the Project window
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);

            // Generate a unique asset path for the new config
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, "GridLayerConfig.asset"));

            // Create and save the new GridLayerConfig asset
            var asset = ScriptableObject.CreateInstance<GridLayerConfig>();
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Focus the Project window and select the new asset
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}