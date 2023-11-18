---
Title: Recommended References
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Table of Contents](#table-of-contents)
- [Goals](#goals)
- [Related rules](#related-rules)
- [Usage](#usage)
- [Settings](#settings)
  - [Opt-Out](#opt-out)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

To have consistency in code-style among the different tools/plugins the use of Analysers is recommended, especially the use of [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers). Additionally code-style analysis using StyleCopy (and code generation in the IDE) should be properly configured using a `stylecop.json`-file as well as `.editorconfig`-file, respectively.

Example-Files can be found at:

* [`stylecop.json`](./examples/StyleCopJson)
* [`.editorconfig`](./examples/Editorconfig)

## Related rules

 * [CCG0005](../rules/ccg0005)
 * [CCG0006](../rules/ccg0006)

These rules are only applied for [project types](../settings#projecttype) `addin` and `module`.

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

<?! Include "../settings/fragments/OmitRecommendedReference.md" /?>

<?! Include "../settings/fragments/OmitRecommendedConfigFile.md" /?>
