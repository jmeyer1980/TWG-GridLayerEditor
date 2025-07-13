// Assets/Editor/TwoDimensionalGridSettup.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

namespace TinyWalnutGames.GridLayerEditor
{
    /// <summary>
    /// Utility class to spawn a Grid and one Tilemap/GameObject per layer.
    /// Supports both side-scrolling (platformer), top-down, isometric, and hexagonal 2D setups.
    /// </summary>
    public static class TwoDimensionalGridSetup
    {
        // Delegate for test injection
        public static System.Action<string[]> CreateCustomGridAction;

        /// <summary>
        /// Enum for platformer layer names.
        /// </summary>
        public enum SideScrollingLayers
        {
            Parallax5,
            Parallax4,
            Parallax3,
            Parallax2,
            Parallax1,
            Background2,
            Background1,
            BackgroundProps,
            WalkableGround,
            WalkableProps,
            Hazards,
            Foreground,
            ForegroundProps,
            RoomMasking,
            Blending,
        }

        /// <summary>
        /// Creates a side-scrolling grid in the scene with platformer layers.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Side-Scrolling Grid")]
        public static void CreateSideScrollingGrid()
        {
            var gridGO = new GameObject("Side-Scrolling Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            int layerCount = Enum.GetValues(typeof(SideScrollingLayers)).Length;
            int index = 0;
            foreach (SideScrollingLayers layer in Enum.GetValues(typeof(SideScrollingLayers)))
            {
                int flippedZ = layerCount - 1 - index;
                CreateTilemapLayer(gridGO.transform, layer.ToString(), flippedZ);
                index++;
            }
        }

        /// <summary>
        /// Context menu for creating a Side-Scrolling Grid from the hierarchy.
        /// Appears when right-clicking in the hierarchy window.
        /// </summary>
        [MenuItem("GameObject/Tiny Walnut Games/Create Side-Scrolling Grid", false, 10)]
        private static void ContextCreateSideScrollingGrid(MenuCommand menuCommand)
        {
            CreateSideScrollingGrid();
        }

        /// <summary>
        /// Enum for top-down layer names.
        /// </summary>
        public enum TopDownLayers
        {
            DeepOcean,
            Ocean,
            ShallowWater,
            Floor,
            FloorProps,
            WalkableGround,
            WalkableProps,
            OverheadProps,
            RoomMasking,
            Blending,
        }

        /// <summary>
        /// Attempts to load a GridLayerConfig asset and returns its layerNames if available.
        /// Otherwise, returns the provided defaultLayers.
        /// </summary>
        private static string[] GetCustomOrDefaultLayers(string[] topDownDefaultLayers)
        {
            var config = AssetDatabase.LoadAssetAtPath<GridLayerConfig>("Assets/GridLayerConfig.asset");
            // Platformer default layers
            string[] platformerDefaultLayers = Enum.GetNames(typeof(SideScrollingLayers));
            if (config != null && config.layerNames != null && config.layerNames.Length > 0)
            {
                // If custom layers are equal to platformer default, use top-down default
                if (config.layerNames.SequenceEqual(platformerDefaultLayers))
                    return topDownDefaultLayers;
                // If custom layers are different from top-down default, use custom
                if (!config.layerNames.SequenceEqual(topDownDefaultLayers))
                    return config.layerNames;
            }
            return topDownDefaultLayers;
        }

        /// <summary>
        /// Creates a top-down grid in the scene with top-down layers.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Default Top-Down Grid")]
        public static void CreateDefaultTopDownGrid()
        {
            var gridGO = new GameObject("Top-Down Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            var layers = GetCustomOrDefaultLayers(Enum.GetNames(typeof(TopDownLayers)));
            int layerCount = layers.Length;
            for (int i = 0; i < layerCount; i++)
            {
                int flippedZ = layerCount - 1 - i;
                CreateTilemapLayer(gridGO.transform, layers[i], flippedZ);
            }
        }

        /// <summary>
        /// Context menu for creating a Top-Down Grid from the hierarchy.
        /// Appears when right-clicking in the hierarchy window.
        /// </summary>
        [MenuItem("GameObject/Tiny Walnut Games/Create Default Top-Down Grid", false, 10)]
        private static void ContextCreateDefaultTopDownGrid(MenuCommand menuCommand)
        {
            CreateDefaultTopDownGrid();
        }

        /// <summary>
        /// Preset layers for isometric and hexagonal top-down grids.
        /// </summary>
        private static readonly string[] IsometricTopDownLayers = new string[]
        {
            "Blending",
            "RoomMasking",
            "OverheadProps",
            "WalkableProps",
            "WalkableGround",
            "FloorProps",
            "Floor",
            "ShallowWater",
            "Ocean",
            "DeepOcean"
        };

        private static readonly string[] HexTopDownLayers = new string[]
        {
            "Blending",
            "RoomMasking",
            "OverheadProps",
            "WalkableProps",
            "WalkableGround",
            "FloorProps",
            "Floor",
            "ShallowWater",
            "Ocean",
            "DeepOcean"
        };

        /// <summary>
        /// Creates an isometric top-down grid in the scene.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Isometric Top-Down Grid")]
        public static void CreateIsometricTopDownGrid()
        {
            var gridGO = new GameObject("Isometric Top-Down Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            var grid = gridGO.GetComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            var layers = GetCustomOrDefaultLayers(IsometricTopDownLayers);
            int layerCount = layers.Length;
            for (int i = 0; i < layerCount; i++)
            {
                int flippedZ = layerCount - 1 - i;
                CreateTilemapLayer(gridGO.transform, layers[i], flippedZ);
            }
        }

        /// <summary>
        /// Context menu for creating an Isometric Top-Down Grid from the hierarchy.
        /// </summary>
        [MenuItem("GameObject/Tiny Walnut Games/Create Isometric Top-Down Grid", false, 10)]
        private static void ContextCreateIsometricTopDownGrid(MenuCommand menuCommand)
        {
            CreateIsometricTopDownGrid();
        }

        /// <summary>
        /// Creates a hexagonal top-down grid in the scene.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Hexagonal Top-Down Grid")]
        public static void CreateHexTopDownGrid()
        {
            var gridGO = new GameObject("Hexagonal Top-Down Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            var grid = gridGO.GetComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Hexagon;
            var layers = GetCustomOrDefaultLayers(HexTopDownLayers);
            int layerCount = layers.Length;
            for (int i = 0; i < layerCount; i++)
            {
                int flippedZ = layerCount - 1 - i;
                CreateTilemapLayer(gridGO.transform, layers[i], flippedZ);
            }
        }

        /// <summary>
        /// Context menu for creating a Hexagonal Top-Down Grid from the hierarchy.
        /// </summary>
        [MenuItem("GameObject/Tiny Walnut Games/Create Hexagonal Top-Down Grid", false, 10)]
        private static void ContextCreateHexTopDownGrid(MenuCommand menuCommand)
        {
            CreateHexTopDownGrid();
        }

        /// <summary>
        /// Creates a grid in the scene using a custom array of layer names.
        /// </summary>
        /// <param name="layerNames">Array of layer names to use for tilemap creation.</param>
        public static void CreateCustomGrid(string[] layerNames)
        {
            if (CreateCustomGridAction != null)
            {
                CreateCustomGridAction(layerNames);
                return;
            }

            var gridGO = new GameObject("Custom Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            int layerCount = layerNames.Length;
            for (int i = 0; i < layerCount; i++)
            {
                int flippedZ = layerCount - 1 - i;
                CreateTilemapLayer(gridGO.transform, layerNames[i], flippedZ);
            }
        }

        /// <summary>
        /// Creates a GameObject under 'parent' with Tilemap & TilemapRenderer,
        /// sets z-offset, Unity layer, sorting layer, and default order.
        /// </summary>
        /// <param name="parent">Parent transform for the new GameObject.</param>
        /// <param name="layerName">Name of the layer for the GameObject.</param>
        /// <param name="zDepth">Z-depth used for positioning.</param>
        private static void CreateTilemapLayer(Transform parent, string layerName, int zDepth)
        {
            var tmGO = new GameObject(layerName, typeof(Tilemap), typeof(TilemapRenderer));
            tmGO.transform.SetParent(parent, worldPositionStays: false);
            tmGO.transform.localPosition = new Vector3(0, 0, zDepth);

            // Try to set Unity layer by name; warn if not found
            int unityLayer = LayerMask.NameToLayer(layerName);
            if (unityLayer != -1)
                tmGO.layer = unityLayer;
            else
                Debug.LogWarning($"Layer '{layerName}' not found. GameObject will use default layer.");

            // Try to set sorting layer by name; warn if not found
            var renderer = tmGO.GetComponent<TilemapRenderer>();
            renderer.sortingLayerName = layerName;
            if (renderer.sortingLayerName != layerName)
                Debug.LogWarning($"Sorting Layer '{layerName}' not found. Renderer will use default sorting layer.");
            renderer.sortingOrder = 0;
        }

        // When creating grids, use config.layerNames for the layers.
        // Example:
        // foreach (var layerName in config.layerNames) { /* use layerName for grid setup */ }
    }
}
