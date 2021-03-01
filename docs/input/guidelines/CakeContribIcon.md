---
Order: 1
Title: CakeContrib-Icon
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Goals](#goals)
- [Related rules](#related-rules)
- [Usage](#usage)
- [Settings](#settings)
  - [Icon-Location](#icon-location)
  - [Icon include in project](#icon-include-in-project)
- [Migrating from an existing project](#migrating-from-an-existing-project)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

* The current cake-contrib icon should be used as icon for the nupkg.
* The icon should be embedded in the nupkg. (See [Reference](https://docs.microsoft.com/en-us/nuget/reference/nuspec#icon))
* iconUrl should - for compatibility reasons - still be added.

## Related rules

 * [CCG0001](../rules/ccg0001)
 * [CCG0002](../rules/ccg0002)
 * [CCG0003](../rules/ccg0003)

## Usage

Using this package automatically enables this guideline.

With no special settings at all (i.e. "The Standard"):
* the current cake-contrib icon will be copied as "icon.png" in the project-directory 
* the icon will be included in the project

## Settings

### Icon-Location
<?! Include "../settings/fragments/IconDestinationLocation.md" /?>

### Icon include in project
<?! Include "../settings/fragments/IconOmitImport.md" /?>

## Migrating from an existing project

* remove the existing icon
* remove the `Include` of the icon from the project-file
* add a reference to CakeContrib.Guidelines
* build the project
* set `PackageIcon` to `icon.png`
