# Tiny Walnut Games Grid Layer System

This package provides a flexible grid layer configuration system for Unity 2D games, supporting platformer, top-down, isometric, and hexagonal grid setups.

## Features

- **GridLayerConfig**: ScriptableObject for storing custom grid layer names.
- **Editor Tools**: Create, edit, and apply grid layer templates via menu and context options.
- **Grid Creation**: Instantly spawn grids with multiple tilemap layers for various genres and layouts.

## Usage

1. **Create a GridLayerConfig asset**  
   - Right-click in the Project window: `Assets > Create > Tiny Walnut Games > Grid Layer Config`
   - Or use the Grid Layer Editor window: `Tiny Walnut Games > Edit Grid Layers`

2. **Edit Layer Names**  
   - Use the Grid Layer Editor window to customize or apply presets.

3. **Create Grids**  
   - Use menu or hierarchy context options under `Tiny Walnut Games` to create:
     - Side-Scrolling Grid
     - Default Top-Down Grid
     - Isometric Top-Down Grid
     - Hexagonal Top-Down Grid

## Assembly Definitions

- **TinyWalnutGames.GridLayerConfig**: Runtime scripts (ScriptableObject).
- **TinyWalnutGames.GridLayerEditor**: Editor scripts (menu, window, grid creation).

## Requirements

- Unity 2021.3 or newer recommended.
- 2D Tilemap package.

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

