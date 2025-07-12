#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace TinyWalnutGames.GridLayerEditor
{
    /// <summary>
    /// Editor window for editing and applying grid layer setups.
    /// Allows users to create, edit, and apply layer configurations for 2D grids.
    /// </summary>
    public class GridLayerEditorWindow : EditorWindow
    {
        /// <summary>
        /// The GridLayerConfig asset currently being edited.
        /// </summary>
        private GridLayerConfig config;

        /// <summary>
        /// Preset layer names for platformer-style grids.
        /// </summary>
        private static readonly string[] PlatformerLayers = new string[]
        {
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
            "Blending",
        };

        /// <summary>
        /// Preset layer names for top-down-style grids.
        /// </summary>
        private static readonly string[] TopDownLayers = new string[]
        {
                "DeepOcean",
                "Ocean",
                "ShallowWater",
                "Floor",
                "FloorProps",
                "WalkableGround",
                "WalkableProps",
                "OverheadProps",
                "RoomMasking",
                "Blending",
        };

        /// <summary>
        /// Array to track selected Unity layers.
        /// </summary>
        private bool[] layerSelections;

        /// <summary>
        /// Array of all Unity layer names.
        /// </summary>
        private string[] unityLayers;

        /// <summary>
        /// The GridLayerConfig asset currently being edited (used for testing purposes).
        /// </summary>
        private GridLayerConfig _config;

        /// <summary>
        /// Allows tests to set the config.
        /// </summary>
        /// <param name="config">The GridLayerConfig to set.</param>
        public void SetConfig(GridLayerConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Allows tests to get the config.
        /// </summary>
        /// <returns>The current GridLayerConfig.</returns>
        public GridLayerConfig GetConfig()
        {
            return _config;
        }

        /// <summary>
        /// Applies a platformer preset to the config.
        /// </summary>
        public void ApplyPlatformerPreset()
        {
            if (_config == null) return;
            _config.layerNames = new[] { "WalkableGround", "Ladders", "Hazards" };
        }

        /// <summary>
        /// Toggles a layer name in the config.
        /// </summary>
        /// <param name="layerName">The name of the layer to toggle.</param>
        /// <param name="enabled">Whether the layer should be enabled or disabled.</param>
        public void ToggleLayer(string layerName, bool enabled)
        {
            if (_config == null) return;
            var names = _config.layerNames ?? new string[0];
            if (enabled)
            {
                if (!System.Array.Exists(names, l => l == layerName))
                {
                    var newNames = new string[names.Length + 1];
                    names.CopyTo(newNames, 0);
                    newNames[names.Length] = layerName;
                    _config.layerNames = newNames;
                }
            }
            else
            {
                _config.layerNames = System.Array.FindAll(names, l => l != layerName);
            }
        }

        /// <summary>
        /// Calls TwoDimensionalGridSetup.CreateCustomGrid with the current config's layer names.
        /// </summary>
        public void CreateGridWithLayers()
        {
            if (_config == null || _config.layerNames == null) return;
            TwoDimensionalGridSetup.CreateCustomGrid(_config.layerNames);
        }

        /// <summary>
        /// Opens the grid layer editor window from the Unity menu.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Edit Grid Layers")]
        public static void ShowWindow()
        {
            GetWindow<GridLayerEditorWindow>("Edit Grid Layers");
        }

        /// <summary>
        /// Called when the editor window is enabled.
        /// </summary>
        private void OnEnable()
        {
            // Get all Unity layers
            unityLayers = GetAllUnityLayerNames();
            layerSelections = new bool[unityLayers.Length];

            // Initialize selections from config
            if (config != null)
            {
                for (int i = 0; i < unityLayers.Length; i++)
                {
                    layerSelections[i] = config.layerNames.Contains(unityLayers[i]);
                }
            }
        }

        /// <summary>
        /// Retrieves all Unity layer names.
        /// </summary>
        /// <returns>Array of Unity layer names.</returns>
        private string[] GetAllUnityLayerNames()
        {
            var layers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string name = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(name))
                    layers.Add(name);
            }
            return layers.ToArray();
        }

        /// <summary>
        /// Draws the editor window GUI for editing grid layer configurations.
        /// </summary>
        private void OnGUI()
        {
            // Field to assign or create a GridLayerConfig asset
            config = (GridLayerConfig)EditorGUILayout.ObjectField("Config Asset", config, typeof(GridLayerConfig), false);

            if (config == null)
            {
                EditorGUILayout.HelpBox("Assign or create a GridLayerConfig asset.", MessageType.Info);
                // Button to create a default platformer config asset
                if (GUILayout.Button("Create Platformer Default Config"))
                {
                    config = CreateInstance<GridLayerConfig>();
                    config.layerNames = (string[])PlatformerLayers.Clone();
                    AssetDatabase.CreateAsset(config, "Assets/GridLayerConfig.asset");
                    AssetDatabase.SaveAssets();
                }
                // Button to create a default top-down config asset
                if (GUILayout.Button("Create Top-Down Default Config"))
                {
                    config = CreateInstance<GridLayerConfig>();
                    config.layerNames = (string[])TopDownLayers.Clone();
                    AssetDatabase.CreateAsset(config, "Assets/GridLayerConfig.asset");
                    AssetDatabase.SaveAssets();
                }
                return;
            }

            EditorGUILayout.LabelField("Edit Layer Names:", EditorStyles.boldLabel);

            // Display and edit the layerNames array from the config asset
            SerializedObject so = new(config);
            SerializedProperty layersProp = so.FindProperty("layerNames");
            EditorGUILayout.PropertyField(layersProp, true);
            so.ApplyModifiedProperties();

            EditorGUILayout.Space();

            // Button to apply platformer preset to the config asset
            if (GUILayout.Button("Apply Platformer Preset"))
            {
                Undo.RecordObject(config, "Apply Platformer Preset");
                config.layerNames = (string[])PlatformerLayers.Clone();
                EditorUtility.SetDirty(config);
            }
            // Button to apply top-down preset to the config asset
            if (GUILayout.Button("Apply Top-Down Preset"))
            {
                Undo.RecordObject(config, "Apply Top-Down Preset");
                config.layerNames = (string[])TopDownLayers.Clone();
                EditorUtility.SetDirty(config);
            }

            EditorGUILayout.Space();

            // Button to create a grid in the scene using the current config's layers
            if (GUILayout.Button("Create Grid With These Layers"))
            {
                TwoDimensionalGridSetup.CreateCustomGrid(config.layerNames);
            }

            EditorGUILayout.Space();

            // Unity layer selection UI
            EditorGUILayout.LabelField("Select Unity Layers for Grid", EditorStyles.boldLabel);
            for (int i = 0; i < unityLayers.Length; i++)
            {
                layerSelections[i] = EditorGUILayout.ToggleLeft(unityLayers[i], layerSelections[i]);
            }

            // Update config.layerNames when selection changes
            if (GUI.changed)
            {
                config.layerNames = unityLayers.Where((layer, idx) => layerSelections[idx]).ToArray();
                EditorUtility.SetDirty(config);
            }

            EditorGUILayout.Space();

            // Recommended layers display
            EditorGUILayout.HelpBox("Recommended Platformer Layers:\n" + string.Join(", ", PlatformerLayers), MessageType.Info);
            EditorGUILayout.HelpBox("Recommended Top Down Layers:\n" + string.Join(", ", TopDownLayers), MessageType.Info);

            EditorGUILayout.Space();

            // Set recommended layers buttons
            if (GUILayout.Button("Set Recommended Platformer Layers"))
            {
                SetRecommendedLayers(PlatformerLayers);
            }
            if (GUILayout.Button("Set Recommended Top Down Layers"))
            {
                SetRecommendedLayers(TopDownLayers);
            }
        }

        /// <summary>
        /// Sets the recommended layers in the config and updates the UI.
        /// </summary>
        /// <param name="recommended">Array of recommended layer names.</param>
        private void SetRecommendedLayers(string[] recommended)
        {
            for (int i = 0; i < unityLayers.Length; i++)
            {
                layerSelections[i] = recommended.Contains(unityLayers[i]);
            }
            config.layerNames = unityLayers.Where((layer, idx) => layerSelections[idx]).ToArray();
            EditorUtility.SetDirty(config);
            Repaint();
        }
    }
}
#endif