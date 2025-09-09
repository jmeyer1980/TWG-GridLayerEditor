#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TinyWalnutGames.GridLayerEditor
{
    /// <summary>
    /// Utility class for managing Unity layers and sorting layers.
    /// Provides functionality to create default layer setups for different game types.
    /// </summary>
    public static class LayerManager
    {
        /// <summary>
        /// Default platformer layers that should exist in Unity's layer manager.
        /// </summary>
        private static readonly string[] PlatformerLayers = new string[]
        {
            "Default",
            "TransparentFX",
            "Ignore Raycast",
            "Water",
            "UI",
            "Parallax5",
            "Parallax4",
            "Parallax3",
            "Parallax2",
            "Parallax1",
            "Background2",
            "Background1",
            "BackgroundProps",
            "WalkableGround",
            "WalkableProps",
            "Hazards",
            "Foreground",
            "ForegroundProps",
            "RoomMasking",
            "Blending"
        };

        /// <summary>
        /// Default top-down layers that should exist in Unity's layer manager.
        /// </summary>
        private static readonly string[] TopDownLayers = new string[]
        {
            "Default",
            "TransparentFX",
            "Ignore Raycast",
            "Water",
            "UI",
            "DeepOcean",
            "Ocean",
            "ShallowWater",
            "Floor",
            "FloorProps",
            "WalkableGround",
            "WalkableProps",
            "OverheadProps",
            "RoomMasking",
            "Blending"
        };

        /// <summary>
        /// Default platformer sorting layers.
        /// </summary>
        private static readonly string[] PlatformerSortingLayers = new string[]
        {
            "Default",
            "Parallax5",
            "Parallax4",
            "Parallax3",
            "Parallax2",
            "Parallax1",
            "Background2",
            "Background1",
            "BackgroundProps",
            "WalkableGround",
            "WalkableProps",
            "Hazards",
            "Foreground",
            "ForegroundProps",
            "RoomMasking",
            "Blending"
        };

        /// <summary>
        /// Default top-down sorting layers.
        /// </summary>
        private static readonly string[] TopDownSortingLayers = new string[]
        {
            "Default",
            "DeepOcean",
            "Ocean",
            "ShallowWater",
            "Floor",
            "FloorProps",
            "WalkableGround",
            "WalkableProps",
            "OverheadProps",
            "RoomMasking",
            "Blending"
        };

        /// <summary>
        /// Creates platformer layers in Unity's layer manager.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Layer Management/Create Platformer Layers")]
        public static void CreatePlatformerLayers()
        {
            CreateLayers(PlatformerLayers, "Platformer");
            CreateSortingLayers(PlatformerSortingLayers, "Platformer");
        }

        /// <summary>
        /// Creates top-down layers in Unity's layer manager.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Layer Management/Create Top-Down Layers")]
        public static void CreateTopDownLayers()
        {
            CreateLayers(TopDownLayers, "Top-Down");
            CreateSortingLayers(TopDownSortingLayers, "Top-Down");
        }

        /// <summary>
        /// Creates all layers (both platformer and top-down).
        /// </summary>
        [MenuItem("Tiny Walnut Games/Layer Management/Create All Layers")]
        public static void CreateAllLayers()
        {
            var allLayers = PlatformerLayers.Union(TopDownLayers).Distinct().ToArray();
            var allSortingLayers = PlatformerSortingLayers.Union(TopDownSortingLayers).Distinct().ToArray();
            
            CreateLayers(allLayers, "All Game Types");
            CreateSortingLayers(allSortingLayers, "All Game Types");
        }

        /// <summary>
        /// Shows a report of current layer usage.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Layer Management/Show Layer Report")]
        public static void ShowLayerReport()
        {
            var usedLayers = GetUsedLayers();
            var usedSortingLayers = GetUsedSortingLayers();
            
            string report = "=== UNITY LAYER REPORT ===\n\n";
            
            report += "UNITY LAYERS:\n";
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    report += $"  [{i:D2}] {layerName}\n";
                }
                else
                {
                    report += $"  [{i:D2}] <empty>\n";
                }
            }
            
            report += "\nSORTING LAYERS:\n";
            foreach (var sortingLayer in usedSortingLayers)
            {
                report += $"  • {sortingLayer}\n";
            }
            
            report += $"\nSUMMARY:\n";
            report += $"  Unity Layers Used: {usedLayers.Count}/32\n";
            report += $"  Sorting Layers Used: {usedSortingLayers.Count}\n";
            
            Debug.Log(report);
            EditorUtility.DisplayDialog("Layer Report", 
                $"Layer report has been logged to the console.\n\n" +
                $"Unity Layers Used: {usedLayers.Count}/32\n" +
                $"Sorting Layers Used: {usedSortingLayers.Count}", 
                "OK");
        }

        /// <summary>
        /// Removes unused layers (with confirmation).
        /// </summary>
        [MenuItem("Tiny Walnut Games/Layer Management/Clean Unused Layers")]
        public static void CleanUnusedLayers()
        {
            if (EditorUtility.DisplayDialog("Clean Unused Layers", 
                "This will remove all Unity layers that are not in the standard presets.\n\n" +
                "This action cannot be undone. Are you sure you want to continue?", 
                "Yes, Clean Layers", "Cancel"))
            {
                CleanLayers();
            }
        }

        /// <summary>
        /// Creates the specified layers in Unity's layer manager.
        /// </summary>
        /// <param name="layerNames">Array of layer names to create.</param>
        /// <param name="setupType">Description of the setup type for logging.</param>
        private static void CreateLayers(string[] layerNames, string setupType)
        {
            var tagManager = GetTagManager();
            if (tagManager == null)
            {
                Debug.LogError("Could not access TagManager. Layer creation failed.");
                return;
            }

            var layersProperty = tagManager.FindProperty("layers");
            var createdLayers = new List<string>();
            var skippedLayers = new List<string>();

            foreach (string layerName in layerNames)
            {
                // Skip built-in layers
                if (IsBuiltInLayer(layerName))
                {
                    continue;
                }

                // Check if layer already exists
                if (LayerMask.NameToLayer(layerName) != -1)
                {
                    skippedLayers.Add(layerName);
                    continue;
                }

                // Find an empty slot
                bool layerCreated = false;
                for (int i = 8; i < 32; i++) // Start at 8 to skip built-in layers
                {
                    var layerProperty = layersProperty.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(layerProperty.stringValue))
                    {
                        layerProperty.stringValue = layerName;
                        createdLayers.Add(layerName);
                        layerCreated = true;
                        break;
                    }
                }

                if (!layerCreated)
                {
                    Debug.LogWarning($"Could not create layer '{layerName}' - no empty slots available.");
                }
            }

            tagManager.ApplyModifiedProperties();

            // Log results
            string message = $"{setupType} Layer Creation Complete:\n";
            if (createdLayers.Count > 0)
            {
                message += $"  Created: {string.Join(", ", createdLayers)}\n";
            }
            if (skippedLayers.Count > 0)
            {
                message += $"  Already Existed: {string.Join(", ", skippedLayers)}\n";
            }

            Debug.Log(message);
            
            if (createdLayers.Count > 0)
            {
                EditorUtility.DisplayDialog("Layers Created", 
                    $"Successfully created {createdLayers.Count} new {setupType.ToLower()} layers.\n\n" +
                    $"Check the console for details.", 
                    "OK");
            }
        }

        /// <summary>
        /// Creates the specified sorting layers.
        /// </summary>
        /// <param name="sortingLayerNames">Array of sorting layer names to create.</param>
        /// <param name="setupType">Description of the setup type for logging.</param>
        private static void CreateSortingLayers(string[] sortingLayerNames, string setupType)
        {
            var tagManager = GetTagManager();
            if (tagManager == null) return;

            var sortingLayersProperty = tagManager.FindProperty("m_SortingLayers");
            var createdSortingLayers = new List<string>();
            var skippedSortingLayers = new List<string>();

            foreach (string sortingLayerName in sortingLayerNames)
            {
                // Check if sorting layer already exists
                if (SortingLayerExists(sortingLayerName))
                {
                    skippedSortingLayers.Add(sortingLayerName);
                    continue;
                }

                // Add new sorting layer
                sortingLayersProperty.InsertArrayElementAtIndex(sortingLayersProperty.arraySize);
                var newSortingLayer = sortingLayersProperty.GetArrayElementAtIndex(sortingLayersProperty.arraySize - 1);
                newSortingLayer.FindPropertyRelative("name").stringValue = sortingLayerName;
                newSortingLayer.FindPropertyRelative("uniqueID").intValue = System.DateTime.Now.GetHashCode();
                
                createdSortingLayers.Add(sortingLayerName);
            }

            tagManager.ApplyModifiedProperties();

            // Log results
            if (createdSortingLayers.Count > 0 || skippedSortingLayers.Count > 0)
            {
                string message = $"{setupType} Sorting Layer Creation Complete:\n";
                if (createdSortingLayers.Count > 0)
                {
                    message += $"  Created: {string.Join(", ", createdSortingLayers)}\n";
                }
                if (skippedSortingLayers.Count > 0)
                {
                    message += $"  Already Existed: {string.Join(", ", skippedSortingLayers)}";
                }

                Debug.Log(message);
            }
        }

        /// <summary>
        /// Gets the TagManager SerializedObject.
        /// </summary>
        /// <returns>SerializedObject for the TagManager, or null if not found.</returns>
        private static SerializedObject GetTagManager()
        {
            var tagManagerAsset = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");
            if (tagManagerAsset == null)
            {
                Debug.LogError("Could not load TagManager.asset");
                return null;
            }
            return new SerializedObject(tagManagerAsset);
        }

        /// <summary>
        /// Checks if the specified layer is a built-in Unity layer.
        /// </summary>
        /// <param name="layerName">Name of the layer to check.</param>
        /// <returns>True if the layer is built-in, false otherwise.</returns>
        private static bool IsBuiltInLayer(string layerName)
        {
            return layerName == "Default" || 
                   layerName == "TransparentFX" || 
                   layerName == "Ignore Raycast" || 
                   layerName == "Water" || 
                   layerName == "UI";
        }

        /// <summary>
        /// Checks if a sorting layer exists.
        /// </summary>
        /// <param name="sortingLayerName">Name of the sorting layer to check.</param>
        /// <returns>True if the sorting layer exists, false otherwise.</returns>
        private static bool SortingLayerExists(string sortingLayerName)
        {
            var sortingLayers = SortingLayer.layers;
            return sortingLayers.Any(layer => layer.name == sortingLayerName);
        }

        /// <summary>
        /// Gets a list of all currently used Unity layers.
        /// </summary>
        /// <returns>List of used layer names.</returns>
        private static List<string> GetUsedLayers()
        {
            var usedLayers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    usedLayers.Add(layerName);
                }
            }
            return usedLayers;
        }

        /// <summary>
        /// Gets a list of all currently used sorting layers.
        /// </summary>
        /// <returns>List of used sorting layer names.</returns>
        private static List<string> GetUsedSortingLayers()
        {
            return SortingLayer.layers.Select(layer => layer.name).ToList();
        }

        /// <summary>
        /// Removes layers that are not in the standard presets.
        /// </summary>
        private static void CleanLayers()
        {
            var tagManager = GetTagManager();
            if (tagManager == null) return;

            var layersProperty = tagManager.FindProperty("layers");
            var standardLayers = PlatformerLayers.Union(TopDownLayers).ToHashSet();
            var removedLayers = new List<string>();

            for (int i = 8; i < 32; i++) // Start at 8 to skip built-in layers
            {
                var layerProperty = layersProperty.GetArrayElementAtIndex(i);
                string layerName = layerProperty.stringValue;
                
                if (!string.IsNullOrEmpty(layerName) && !standardLayers.Contains(layerName))
                {
                    layerProperty.stringValue = "";
                    removedLayers.Add(layerName);
                }
            }

            tagManager.ApplyModifiedProperties();

            if (removedLayers.Count > 0)
            {
                string message = $"Cleaned {removedLayers.Count} unused layers:\n  {string.Join(", ", removedLayers)}";
                Debug.Log(message);
                EditorUtility.DisplayDialog("Layers Cleaned", 
                    $"Removed {removedLayers.Count} unused layers.\n\nCheck the console for details.", 
                    "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("No Cleanup Needed", 
                    "All layers are either built-in or part of the standard presets.", 
                    "OK");
            }
        }

        /// <summary>
        /// Validates that all layers from a GridLayerConfig exist in Unity.
        /// Creates missing layers if requested.
        /// </summary>
        /// <param name="config">The GridLayerConfig to validate.</param>
        /// <param name="createMissing">Whether to create missing layers automatically.</param>
        /// <returns>True if all layers exist (or were created), false otherwise.</returns>
        public static bool ValidateConfigLayers(GridLayerConfig config, bool createMissing = false)
        {
            if (config == null || config.layerNames == null) return true;

            var missingLayers = new List<string>();
            
            foreach (string layerName in config.layerNames)
            {
                if (LayerMask.NameToLayer(layerName) == -1 && !IsBuiltInLayer(layerName))
                {
                    missingLayers.Add(layerName);
                }
            }

            if (missingLayers.Count == 0) return true;

            if (createMissing)
            {
                CreateLayers(missingLayers.ToArray(), "Config Validation");
                return true;
            }

            Debug.LogWarning($"Missing Unity layers: {string.Join(", ", missingLayers)}");
            return false;
        }
    }
}
#endif
