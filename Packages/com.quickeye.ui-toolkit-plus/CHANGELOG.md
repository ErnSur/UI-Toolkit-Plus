## [1.10.0] - 2022-11-2

## Added
- `ToolbarDropdownButton`
- Added USS class name fields to Tab class family

### Changed
- Added "qe-" prefix to `Tab`, `TabDropdown` and `TabGroup` class names
- Modified `Tab` and `TabDropdown` layout and style for more polished look

### Fixed
- Missing icon of `TabDropdown` in runtime

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