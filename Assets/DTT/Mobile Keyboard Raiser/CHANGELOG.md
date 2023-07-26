# Changelog

All notable changes to this package will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and this package adheres to [Semantic Versioning](https://semver.org/)

## [2.1.3] - 2022-11-08
### Fixed
- PixelHeight would return wrong values due to keyboard raising event.

## [2.1.2] - 2022-05-17
### Updated
- Updated dependencies.
### Removed
- Removed changelog from documentation.

## [2.1.1] - 2022-01-27
### Updated
- Folder structure for the example scenes.

### Added
- Config file holding the debug settings of the keyboard raiser.

### Fixed
- Undoing debug values in the inspector of the keyboard raiser component.

## [2.1.0] - 2022-01-25
### Updated
- Updated dependencies to editor utilities, runtime utilities and publishing tools.

## [2.0.1] - 2022-01-13
### Fixed
- Error building on platform that isn't Android or iOS.

## [2.0.0] - 2022-01-13
### Updated
- Updated dependencies to editor utilities, runtime utilities and publishing tools.

## [1.0.0] - 2021-12-14
### Added
 - Being able to retrieve keyboard state for Android and iOS.
 - Unit tests.
 - Component that lets you accurately raise the UI based on when the keyboard becomes active.
 - Global instance that lets you hook into Unity's update loop.