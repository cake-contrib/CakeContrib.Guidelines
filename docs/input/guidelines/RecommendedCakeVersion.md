---
Title: Recommended Cake Version
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Goals](#goals)
- [Related rules](#related-rules)
- [Usage](#usage)
- [Settings](#settings)
  - [Opt-Out](#opt-out)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

For addins and modules it is recommended to reference the lowest version of Cake with API compatibility to the latest version.
This is currently version 1.0.0.

## Related rules

 * [CCG0009](../rules/ccg0009)

This rule is only applied for [project types](../settings#projecttype) `addin` and `module`.

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

<?! Include "../settings/fragments/OmitRecommendedCakeVersion.md" /?>
