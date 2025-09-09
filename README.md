# Tiny Walnut Games Grid Layer System

Tutorial video: https://youtu.be/z6JuMheYuuc?si=yRFe3AO61SIR7Yvg

This package provides a flexible grid layer configuration system for Unity 2D games, supporting platformer, top-down, isometric, and hexagonal grid setups. **NEW in v1.1.0**: Automatic layer creation and improved columnar UI!

## ✨ Features

- **GridLayerConfig**: ScriptableObject for storing custom grid layer names.
- **Editor Tools**: Create, edit, and apply grid layer templates via menu and context options.
- **Grid Creation**: Instantly spawn grids with multiple tilemap layers for various genres and layouts.
- **🆕 LayerManager**: Automatically creates Unity layers and sorting layers for you!
- **🆕 Columnar UI**: Improved Grid Layer Editor with better space utilization.
- **🆕 Smart Validation**: Ensures all layers exist before creating grids.

## 🚀 Installation

1. **Open your Unity project**

2. **Import the package using one of the below methods**
   - Use the github URL: `https://github.com/jmeyer1980/TWG-GridLayerEditor`
   - Use the NuGet package using the dotnet/nuget cmd prompt: `dotnet add package TWG.GridLayerEditor --version 1.1.0`

## 🎯 Quick Start

### 1. **🆕 Automatic Layer Setup (Recommended)**
Instead of manually creating layers, use the new LayerManager:
- **For Platformers**: `Tiny Walnut Games > Layer Management > Create Platformer Layers`
- **For Top-Down Games**: `Tiny Walnut Games > Layer Management > Create Top-Down Layers`
- **For All Types**: `Tiny Walnut Games > Layer Management > Create All Layers`

This automatically creates all necessary Unity layers AND sorting layers for you!

### 2. **Manual Layer Setup (Legacy)**
If you prefer manual setup, add these layers to Unity's Layer Manager:
   - **Platformer Layers**:
     - Parallax5, Parallax4, Parallax3, Parallax2, Parallax1
     - Background2, Background1, BackgroundProps
     - WalkableGround, WalkableProps, Hazards
     - Foreground, ForegroundProps, RoomMasking, Blending
   
   - **Top-Down Layers**:
     - DeepOcean, Ocean, ShallowWater
     - Floor, FloorProps
     - WalkableGround, WalkableProps
     - OverheadProps, RoomMasking, Blending

## 📋 Usage

1. **Create a GridLayerConfig asset**  
   - Right-click in the Project window: `Assets > Create > Tiny Walnut Games > Grid Layer Config`
   - Or use the Grid Layer Editor window: `Tiny Walnut Games > Edit Grid Layers`

2. **🆕 Edit Layer Names with New UI**  
   - Open the improved Grid Layer Editor window: `Tiny Walnut Games > Edit Grid Layers`
   - **Left Panel**: Select Unity layers with easy-to-use checkboxes
   - **Right Panel**: Configure settings and apply presets
   - **Quick Buttons**: "Select All" / "Select None" for rapid selection  
   - Use menu or hierarchy context options under `Tiny Walnut Games` to create:
     - Side-Scrolling Grid
     - Default Top-Down Grid
     - Isometric Top-Down Grid
     - Hexagonal Top-Down Grid
   - **🆕 Or** use "Create Grid With Selected Layers" in the editor window for custom configurations

## 🛠️ New Layer Management Tools

### Layer Creation Tools
- `Tiny Walnut Games > Layer Management > Create Platformer Layers`
- `Tiny Walnut Games > Layer Management > Create Top-Down Layers`  
- `Tiny Walnut Games > Layer Management > Create All Layers`

### Maintenance Tools
- `Tiny Walnut Games > Layer Management > Show Layer Report` - Analyze layer usage
- `Tiny Walnut Games > Layer Management > Clean Unused Layers` - Remove non-standard layers

### Validation
The system now automatically validates that all layers exist when creating grids and can create missing layers for you!

## Assembly Definitions

- **TinyWalnutGames.GridLayerEditor**: Editor scripts (menu, window, grid creation).

## Requirements

- Unity 2021.3 or newer recommended.
- 2D Tilemap package.
- 2D Tilemap Extras package (recommended but not required).

## Example

1. Create a new GridLayerConfig asset.
2. Edit layers as needed.
3. Use the editor menu or right-click in the hierarchy to create a grid with your layers.

# Grid Layer Editor Workflow

## Recommended Layers

**Platformer Layers:**
Blending, RoomMasking, ForegroundProps, Foreground, WalkableProps, Hazards, WalkableGround, BackgroundProps, Background1, Background2, Parallax1, Parallax2, Parallax3, Parallax4, Parallax5

**Top Down Layers:**
Blending, RoomMasking, ForegroundProps, Foreground, WalkableProps, Hazards, WalkableGround, BackgroundProps, Background1, Background2

## Workflow

1. Open the Grid Layer Editor Window.
2. Select Unity layers using the multiselect list.
3. Use the "Set Recommended Platformer Layers" or "Set Recommended Top Down Layers" buttons to quickly fill selections.
4. Create grids using the selected layers.

Selected layers are stored in the GridLayerConfig asset and used for grid creation.

## License

MIT license, which can be found at https://opensource.org/license/mit/

In layman's terms, you can use this code in your projects, modify it, and share it, as long as you include the original license:

Copyright (c) 2025 Tiny Walnut Games
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

