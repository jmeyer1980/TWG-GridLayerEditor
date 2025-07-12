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
                "Blending",
                "RoomMasking",
                "ForegroundProps",
                "Foreground",
                "WalkableProps",
                "Hazards",
                "WalkableGround",
                "BackgroundProps",
                "Background1",
                "Background2",
                "Parallax1",
                "Parallax2",
                "Parallax3",
                "Parallax4",
                "Parallax5"
        };
    }
}