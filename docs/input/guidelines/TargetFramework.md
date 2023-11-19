---
Title: Target Frameworks
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Goals](#goals)
  - [Required / Suggested versions](#required--suggested-versions)
- [Related rules](#related-rules)
- [Usage](#usage)
- [Settings](#settings)
  - [Cake Version](#cake-version)
  - [Opt-Out](#opt-out)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

Each addin/module should have maximum compatibility when being used. Toward that end some Framework versions are required and some others are
suggested, depending on the Cake.Core version that is being referenced.

### Required / Suggested versions

Depending on the package type and the referenced `Cake.Core`-version different target versions are required and/or suggested.
Missing a required target version will raise [CCG0007](../rules/ccg0007) as an error
while missing a suggested target version will raise [CCG0007](../rules/ccg0007) as a warning.

* Package type: addin
  * Cake.Core < 1.0.0
    * Required: `netstandard2.0`
    * Suggested: `net461`
      * alternative: `net46`
  * Cake.Core >= 1.0.0
    * Required: `netstandard2.0`
    * Suggested: `net461`
      * alternative: `net46`
    * Suggested: `net5.0`
  * Cake.Core >= 2.0.0
    * Required: `netcoreapp3.1`
    * Required: `net5.0`
    * Required: `net6.0`
  * Cake.Core >= 3.0.0
    * Required: `net6.0`
    * Required: `net7.0`
* Package type: module
  * Cake.Core < 2.0.0
    * Required: `netstandard2.0`
    * No additional targets are allowed.
  * Cake.Core >= 2.0.0
    * Required: `netcoreapp3.1`
    * No additional targets are allowed.
  * Cake.Core >= 3.0.0
    * Required: `net6.0`
    * No additional targets are allowed.

For package type recipe no framework reference is required or suggested.

## Related rules

 * [CCG0007](../rules/ccg0007)

These rules are only applied for [project types](../settings#projecttype) `addin` and `module`.

## Usage

Using this package automatically enables this guideline.

## Settings

### Cake Version

<?! Include "../settings/fragments/OverrideCakeVersion.md" /?>

### Opt-Out

<?! Include "../settings/fragments/OmitTargetFramework.md" /?>
