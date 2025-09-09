# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2025-09-09

### Added
- **LayerManager**: Comprehensive layer management system for creating Unity layers and sorting layers
- **Columnar Layout**: Grid Layer Editor window now uses a two-column layout for better space utilization
- **Scroll Views**: Added scroll areas to prevent infinite window expansion with many layers
- **Quick Selection**: "Select All" and "Select None" buttons for layer toggles
- **Layer Validation**: Automatic validation of GridLayerConfig assets
- **Layer Reports**: Tools to analyze current layer usage in projects
- **Layer Cleanup**: Safe removal of unused layers with confirmation dialogs
- **Better File Dialogs**: Config creation now allows choosing save location
- **Enhanced Presets**: Improved preset application with undo support

### Changed
- **UI Layout**: Redesigned Grid Layer Editor window with left/right column split
- **Window Sizing**: Set minimum window size for better layout consistency
- **Layer Display**: Compact display of active layers with smart truncation
- **Error Handling**: Improved error messages and validation feedback
- **Documentation**: Comprehensive inline documentation for all methods

### Fixed
- **Window Expansion**: Fixed infinite vertical expansion when many layers are present
- **Layer Synchronization**: Better synchronization between layer selections and config
- **Performance**: Reduced UI refreshing overhead with targeted updates
- **Undo Support**: All operations now properly support Unity's undo system

### Technical
- **Sacred Symbols Compliance**: All new code follows meaningful naming and non-destructive principles
- **Single Responsibility**: Methods focused on single, clear purposes
- **Memory Management**: Efficient handling of scroll positions and UI state

## [1.0.1] - 2024-XX-XX

### Fixed
- Layer selection synchronization issues
- Documentation improvements

## [1.0.0] - 2024-XX-XX

### Added
- Initial release
- Basic grid creation functionality
- Platformer and top-down presets
- Grid Layer Editor window
- Support for side-scrolling, top-down, isometric, and hexagonal grids
- GridLayerConfig ScriptableObject for saving layer configurations
- Menu items for quick grid creation

### Features
- **TwoDimensionalGridSetup**: Core grid creation utilities
- **GridLayerConfig**: Configuration asset system
- **GridLayerEditorWindow**: Visual editor for layer selection
- **Multiple Grid Types**: Rectangle, isometric, hexagonal support
- **Preset Systems**: Built-in presets for common game types
