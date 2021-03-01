---
Order: 2
Title: PrivateAssets in references
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

As the recommendation from upstream is always to have `Cake.Core` and `Cake.Common` set as private assets, this is checked on build.
Additionally, references to `CakeContrib.Guidelines` and `Cake.Addin.Analyzer` are also checked.

## Related rules

 * [CCG0004](../rules/ccg0004)

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

<?! Include "../settings/fragments/OmitPrivateCheck.md" /?>
