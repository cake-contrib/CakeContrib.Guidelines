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
* if no `PackageIcon` property is specified, a default will be assigned.
* if no icon (matching the `PackageIcon` property) is referenced in the project, a default will be referenced.
* if the referenced icon is not binary equal to the default icon, it will be updated.
* if no `PackageIconUrl` property is specified, a default will be assigned.

## Settings

### Icon include in project
<?! Include "../settings/fragments/IconOmitImport.md" /?>

## Migrating from an existing project

No steps are needed anymore. Existing settings will be detected and honored.

Optionally:
* remove the existing icon
* remove the `Include` of the icon from the project-file
* remove the `PackageIcon` from the project-file
* remove the `PackageIconUrl` from the project-file
