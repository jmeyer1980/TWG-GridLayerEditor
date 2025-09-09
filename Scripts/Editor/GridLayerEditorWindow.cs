#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        /// Scroll position for the layer selection area.
        /// </summary>
        private Vector2 layerScrollPosition;

        /// <summary>
        /// Scroll position for the configuration area.
        /// </summary>
        private Vector2 configScrollPosition;

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
            var window = GetWindow<GridLayerEditorWindow>("Edit Grid Layers");
            window.minSize = new Vector2(600, 400); // Set minimum size for column layout
        }

        /// <summary>
        /// Called when the editor window is enabled.
        /// </summary>
        private void OnEnable()
        {
            // Get all Unity layers
            unityLayers = GetAllUnityLayerNames();
            layerSelections = new bool[unityLayers.Length];

            // Initialize scroll positions
            layerScrollPosition = Vector2.zero;
            configScrollPosition = Vector2.zero;

            // Initialize selections from config
            UpdateLayerSelections();
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
            // Main horizontal split layout
            EditorGUILayout.BeginHorizontal();

            // Left column - Layer Selection (40% of window width)
            DrawLayerSelectionColumn();

            // Right column - Configuration Controls (60% of window width)
            DrawConfigurationColumn();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the left column containing Unity layer selection toggles.
        /// </summary>
        private void DrawLayerSelectionColumn()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f));

            EditorGUILayout.LabelField("Select Unity Layers", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Scrollable area for layer toggles with fixed height
            layerScrollPosition = EditorGUILayout.BeginScrollView(layerScrollPosition, GUILayout.Height(position.height - 100));

            if (unityLayers != null && layerSelections != null)
            {
                bool changed = false;
                for (int i = 0; i < unityLayers.Length; i++)
                {
                    bool newValue = EditorGUILayout.ToggleLeft(unityLayers[i], layerSelections[i]);
                    if (newValue != layerSelections[i])
                    {
                        layerSelections[i] = newValue;
                        changed = true;
                    }
                }

                // Update config when selections change
                if (changed && config != null)
                {
                    Undo.RecordObject(config, "Update Layer Selection");
                    config.layerNames = unityLayers.Where((layer, idx) => layerSelections[idx]).ToArray();
                    EditorUtility.SetDirty(config);
                }
            }

            EditorGUILayout.EndScrollView();

            // Quick selection buttons at bottom of left column
            EditorGUILayout.Space();
            if (GUILayout.Button("Select All"))
            {
                SelectAllLayers(true);
            }
            if (GUILayout.Button("Select None"))
            {
                SelectAllLayers(false);
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the right column containing configuration controls.
        /// </summary>
        private void DrawConfigurationColumn()
        {
            EditorGUILayout.BeginVertical();

            // Config asset field
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
            config = (GridLayerConfig)EditorGUILayout.ObjectField("Config Asset", config, typeof(GridLayerConfig), false);

            if (config == null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Assign or create a GridLayerConfig asset to begin editing layers.", MessageType.Info);
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Create New Config:", EditorStyles.boldLabel);
                
                if (GUILayout.Button("Create Platformer Default Config"))
                {
                    CreateDefaultConfig(PlatformerLayers, "Platformer");
                }
                if (GUILayout.Button("Create Top-Down Default Config"))
                {
                    CreateDefaultConfig(TopDownLayers, "TopDown");
                }
                
                EditorGUILayout.EndVertical();
                return;
            }

            // Scrollable configuration area
            configScrollPosition = EditorGUILayout.BeginScrollView(configScrollPosition);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Layer Configuration", EditorStyles.boldLabel);

            // Display current layer names in a compact format
            if (config.layerNames != null && config.layerNames.Length > 0)
            {
                EditorGUILayout.LabelField($"Active Layers ({config.layerNames.Length}):", EditorStyles.miniLabel);
                string layerList = string.Join(", ", config.layerNames.Take(5));
                if (config.layerNames.Length > 5)
                    layerList += $"... (+{config.layerNames.Length - 5} more)";
                EditorGUILayout.LabelField(layerList, EditorStyles.wordWrappedMiniLabel);
            }
            else
            {
                EditorGUILayout.LabelField("No layers selected", EditorStyles.miniLabel);
            }

            EditorGUILayout.Space();

            // Preset buttons
            EditorGUILayout.LabelField("Apply Presets:", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Platformer Preset"))
            {
                ApplyPreset(PlatformerLayers, "Apply Platformer Preset");
            }
            if (GUILayout.Button("Top-Down Preset"))
            {
                ApplyPreset(TopDownLayers, "Apply Top-Down Preset");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Recommended layers info (compact)
            EditorGUILayout.LabelField("Recommended Layers:", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Platformer:", EditorStyles.miniLabel);
            EditorGUILayout.LabelField(string.Join(", ", PlatformerLayers.Take(8)) + "...", EditorStyles.wordWrappedMiniLabel);
            
            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Top-Down:", EditorStyles.miniLabel);
            EditorGUILayout.LabelField(string.Join(", ", TopDownLayers.Take(8)) + "...", EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Action buttons
            EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Create Grid With Selected Layers", GUILayout.Height(30)))
            {
                if (config.layerNames != null && config.layerNames.Length > 0)
                {
                    TwoDimensionalGridSetup.CreateCustomGrid(config.layerNames);
                }
                else
                {
                    EditorUtility.DisplayDialog("No Layers Selected", "Please select at least one layer before creating a grid.", "OK");
                }
            }

            EditorGUILayout.Space();

            // Advanced editor (collapsible)
            EditorGUILayout.LabelField("Advanced Editor:", EditorStyles.boldLabel);
            SerializedObject so = new SerializedObject(config);
            SerializedProperty layersProp = so.FindProperty("layerNames");
            EditorGUILayout.PropertyField(layersProp, new GUIContent("Layer Names Array"), true);
            
            if (so.ApplyModifiedProperties())
            {
                // Update layer selections when array is modified directly
                UpdateLayerSelections();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Creates a default configuration asset with the specified layers.
        /// </summary>
        /// <param name="defaultLayers">Array of default layer names.</param>
        /// <param name="configType">Type identifier for the config (e.g., "Platformer", "TopDown").</param>
        private void CreateDefaultConfig(string[] defaultLayers, string configType)
        {
            string path = EditorUtility.SaveFilePanelInProject(
                $"Create {configType} Grid Layer Config", 
                $"GridLayerConfig_{configType}", 
                "asset", 
                $"Choose location for {configType} Grid Layer Config");
                
            if (!string.IsNullOrEmpty(path))
            {
                config = CreateInstance<GridLayerConfig>();
                config.layerNames = (string[])defaultLayers.Clone();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                UpdateLayerSelections();
            }
        }

        /// <summary>
        /// Applies a preset to the current configuration.
        /// </summary>
        /// <param name="presetLayers">Array of preset layer names.</param>
        /// <param name="undoName">Name for the undo operation.</param>
        private void ApplyPreset(string[] presetLayers, string undoName)
        {
            if (config == null) return;
            
            Undo.RecordObject(config, undoName);
            config.layerNames = (string[])presetLayers.Clone();
            EditorUtility.SetDirty(config);
            UpdateLayerSelections();
        }

        /// <summary>
        /// Selects or deselects all layers.
        /// </summary>
        /// <param name="selectAll">True to select all layers, false to deselect all.</param>
        private void SelectAllLayers(bool selectAll)
        {
            if (config == null || layerSelections == null) return;
            
            Undo.RecordObject(config, selectAll ? "Select All Layers" : "Deselect All Layers");
            
            for (int i = 0; i < layerSelections.Length; i++)
            {
                layerSelections[i] = selectAll;
            }
            
            if (selectAll)
            {
                config.layerNames = (string[])unityLayers.Clone();
            }
            else
            {
                config.layerNames = new string[0];
            }
            
            EditorUtility.SetDirty(config);
        }

        /// <summary>
        /// Updates the layer selection checkboxes based on the current config.
        /// </summary>
        private void UpdateLayerSelections()
        {
            if (config == null || unityLayers == null || layerSelections == null) return;
            
            for (int i = 0; i < unityLayers.Length; i++)
            {
                layerSelections[i] = config.layerNames != null && config.layerNames.Contains(unityLayers[i]);
            }
            
            Repaint();
        }

    }
}
#endif