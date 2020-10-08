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

To have consistency in code-style among the different tools/plugins the use of Analysers is recommended, especially the use of [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers). Additionally code-style anlysis using StyleCopy (and code generation in the IDE) should be properly configured using a `stylecop.json`-file as well as `.editorconfig`-file, respectively.

Example-Files can be found at:

* [`stylecop.json`](./examples/StyleCopJson)
* [`.editorconfig`](./examples/Editorconfig)

## Related rules

 * [CCG0005](../rules/ccg0005)
 * [CCG0006](../rules/ccg0006)

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

It is also possible to opt-out of the check for configuration-files (`stylecop.json` as well as `.editorconfig`)
using the following settings:
```xml
<ItemGroup>
    <CakeContribGuidelinesOmitRecommendedConfigFile Include="stylecop.json" />
    <CakeContribGuidelinesOmitRecommendedConfigFile Include=".editorconfig" />
</ItemGroup>
```
