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