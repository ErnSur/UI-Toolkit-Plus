## [3.0.2] - 2022-12-10

## Fixed
- Moved Editor element `ToolbarDropdownButton` to Editor assembly.


## [3.0.1] - 2022-11-27

## Added
- Package description

## Fixed
- Fixed scene references in Reorderables Sample
- Fixed missing entry for Tabs Samples in package.json

## [3.0.0] - 2022-11-25

## Added
- UXML Code Generation:
  - Option to override namespace for generated C# files from UXML importer header.
  - Option to generate a second file of a generated partial class.
  - Project-wide settings for code generation.
    - support for pascal case and camel case styles
    - support for prefix and suffix for code identifiers
  - Custom icon for .gen.cs files.

## Changed
- generate c# script context action moved to dropdown menu in UXML Importer header.
- uxml attribute rename: `code-gen-prefix` -> `gen-cs-private-field-prefix`

## [2.0.0] - 2022-11-2

## Added
- `ToolbarDropdownButton`
- Added USS class name fields to Tab class family
- Vertical Tab group style changes

### Changed
- Updated `CompatibilityExtensions`
  - Support for `BaseVerticalCollectionView.itemsChosen`
  - Support for `BaseVerticalCollectionView.selectionChanged`
- Renamed `Tab` UXML attribute: `Reorderable` to `is-reorderable`
- Added "qe-" prefix to Tab family USS selectors
- Modified Tab family layout and style for more polished look and behavior

### Fixed
- Missing icon of `TabDropdown` in runtime
- Fixed Tab `reorderable` UXML attribute resetting after saving changes in UI Builder. [issue description](https://forum.unity.com/threads/uxmltraits-and-custom-attributes-resetting-in-inspector.966215/#post-6311601)

## [1.9.0] - 2022-10-31

## Added
- You can set `TabGroup` mode from UI Builder now.
- `TabGroup` has a new scroller child.
- `TabDropdown` dropdown button is now hidden when there is no action registered.
- Reordering in vertical `TabGroup` now preserves element layout.

### Changed
- `Reorderable` now wraps target in new visual element when dragging.
- `Tab.Label` from `protected` to `private`.

### Fixed
- Fixed visual glitch of tabs jumping when reorderable drag started.
- Fixed `TabGroup` scroller visibility in newer versions of Unity.