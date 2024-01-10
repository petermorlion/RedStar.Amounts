# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [4.1.5] - 2022-10-17

### Fixed

- Fix readme issues

## [4.1.4] - 2022-10-17

### Fixed

- Add readme to project so it shows up in nuget.org

## [4.1.3] - 2022-10-17

### Fixed

- AppVeyor image

### Changed

- Updated packages

## [4.1.2] - 2019-11-06

### Changed

- Updated packages

## [4.1.1] - 2019-07-10

### Added

- Added documentation in source control
- Added NDepend
- Added code of conduct

### Fixed

- NuGet details

### Changed

- Updated packages
- Make test assemblies .NET Core 2.1 projects

## [4.1.0] - 2018-10-05

### Added

- Added SourceLink

## [4.0.0] - 2018-09-11

### Added

- Add extra tags and description for StandardUnits

### Changed

- Make addition/subtraction with null result in null
- Changes some test method names

## [3.0.1] - 2018-06-25

### Fixed

- Fix type in volume units
- Fix micro symbol in units

## [3.0.0] - 2018-06-25

### Added

- Added dyne unit
- Added extra metric prefixes

### Changed

- Changed symbol of ampère to A instead of amp

## [2.1.1] - 2018-02-21

### Fixed

- Correct version

## [2.1.0] - 2018-02-21

### Added

- Added tests for math methods
- Added more XML comments
- Added extra Average method
- Added solution itmes
- Added URLs to NuGet packages

## [2.0.2] - 2017-11-06

### Fixed

- Add NuGet packages to artifacts

## [2.0.1] - 2017-11-06

### Fixed

- Deploy on tag

## [2.0.0] - 2017-11-06

### Added

- Deploy to NuGet from AppVeyor
- Added more tests

### Changed

- Move to .NET Standard 2.0

### Removed

- Removed old serialization
- Removed AssemblyInfo.cs files
- Removed commented tests

## [1.5.0] - 2016-05-13

### Added

- Added JSON Object serialization

## [1.4.0] - 2016-05-12

### Added

- Added JSON.Net serialization support

## [1.3.1] - 2016-05-12

### Fixed

- Broken link in readme
- Make registering and getting units thread-safe

## [1.3.0] - 2016-05-12

### Added

- Added Limit method
- Added Average and Sum methods
- Added AmountMath.Abs method

### Fixed

- Fixed serializing complex units
- Fixed serializing and deserializing multiple times

## [1.2.0] - 2016-05-11

### Added

- Added rounding
- Allow registering with Unit.None as base type
- Added parsing
- Added AmountMath with Min and Max

### Removed

- Removed exception when parsing units

## [1.1.0] - 2016-05-11

### Changed

- Performance improvement: if units are the same, no conversion is needed to compare

## [1.0.0] - 2016-05-11

Initial version, originally written by Rudi Breedenraedt. Made some small improvements.
