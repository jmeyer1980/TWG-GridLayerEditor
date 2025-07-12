// Assets/Editor/TwoDimensionalGridSettup.cs
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

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
            Blending,
            RoomMasking,
            ForegroundProps,
            Hazards,
            WalkableProps,
            WalkableGround,
            BackgroundProps,
            Background1,
            Background2,
            Foreground,
            Parallax1,
            Parallax2,
            Parallax3,
            Parallax4,
            Parallax5,
        }

        /// <summary>
        /// Creates a side-scrolling grid in the scene with platformer layers.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Side-Scrolling Grid")]
        public static void CreateSideScrollingGrid()
        {
            var gridGO = new GameObject("Side-Scrolling Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            int index = 0;
            foreach (SideScrollingLayers layer in Enum.GetValues(typeof(SideScrollingLayers)))
            {
                CreateTilemapLayer(gridGO.transform, layer.ToString(), index++);
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
            Blending,
            RoomMasking,
            OverheadProps,
            WalkableProps,
            WalkableGround,
            FloorProps,
            Floor,
            ShallowWater,
            Ocean,
            DeepOcean
        }

        /// <summary>
        /// Creates a top-down grid in the scene with top-down layers.
        /// </summary>
        [MenuItem("Tiny Walnut Games/Create Default Top-Down Grid")]

        public static void CreateDefaultTopDownGrid()
        {
            var gridGO = new GameObject("Top-Down Grid", typeof(Grid));
            gridGO.transform.position = Vector3.zero;
            int index = 0;
            foreach (TopDownLayers layer in Enum.GetValues(typeof(TopDownLayers)))
            {
                CreateTilemapLayer(gridGO.transform, layer.ToString(), index++);
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
            for (int i = 0; i < IsometricTopDownLayers.Length; i++)
            {
                CreateTilemapLayer(gridGO.transform, IsometricTopDownLayers[i], i);
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
            for (int i = 0; i < HexTopDownLayers.Length; i++)
            {
                CreateTilemapLayer(gridGO.transform, HexTopDownLayers[i], i);
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
            for (int i = 0; i < layerNames.Length; i++)
            {
                CreateTilemapLayer(gridGO.transform, layerNames[i], i);
            }
        }

        /// <summary>
        /// Creates a GameObject under 'parent' with Tilemap & TilemapRenderer,
        /// sets z-offset, Unity layer, sorting layer, and default order.
        /// </summary>
        /// <param name="parent">Parent transform for the new GameObject.</param>
        /// <param name="layerName">Name of the layer for the GameObject.</param>
        /// <param name="index">Index used for z-offset.</param>
        private static void CreateTilemapLayer(Transform parent, string layerName, int index)
        {
            var tmGO = new GameObject(layerName, typeof(Tilemap), typeof(TilemapRenderer));
            tmGO.transform.SetParent(parent, worldPositionStays: false);
            tmGO.transform.localPosition = new Vector3(0, 0, -index);

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
