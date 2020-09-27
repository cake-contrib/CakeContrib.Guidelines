---
Order: 3
Title: Recommended References
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

To have consistency in code-style among the different tools/plugins the use of Analysers is recommended, especially the use of [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers).

## Related rules

 * [CCG0005](../rules/ccg0005)

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

It it possible to opt-out of the check for StyleCop using the following setting:

(*Keep in mind, though that it is not recommended to opt-out of this feature*)

```xml
<ItemGroup>
    <CakeContribGuidelinesOmitRecommendedReference Include="StyleCop.Analyzers" />
</ItemGroup>
```
