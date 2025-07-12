using UnityEngine;

namespace TinyWalnutGames.GridLayerEditor
{    /// <summary>
    /// ScriptableObject to store layer names for grid setups.
    /// Used by editor scripts to configure and create grids with custom layers.
    /// </summary>
    [CreateAssetMenu(fileName = "GridLayerConfig", menuName = "Tiny Walnut Games/Grid Layer Config")]
    public class GridLayerConfig : ScriptableObject
    {
        /// <summary>
        /// Array of layer names to use for grid creation.
        /// </summary>
        public string[] layerNames = new string[]
        {
            // Default to platformer layers
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
    }
}