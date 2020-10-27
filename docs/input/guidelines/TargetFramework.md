---
Order: 4
Title: Target Frameworks
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

As .NET Framework < 4.7.2 has issues with running .NET Standard assemblies, and Cake itself can run on .NET Framework 4.6.1 it is suggested to multi-target addins to `netstandard2.0` and `net461` to have the maximum compatibility.

### Required / Suggested versions

Depending on the referenced `Cake.Core`-version different target versions are required and/or suggested.
Missing a required target version will raise [CCG0007](../rules/ccg0007) as an error
while missing a suggested target version will raise [CCG0007](../rules/ccg0007) as a warning.

* Cake.Core <= 0.33.0
  * Required: `netstandard2.0`
  * Suggested: `net461`
    * alternative: `net46`

## Related rules

 * [CCG0007](../rules/ccg0007)

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

It it possible to opt-out of the check for target framework using the following setting:

(*Keep in mind, though that it is not recommended to opt-out of this feature*)

```xml
<ItemGroup>
    <CakeContribGuidelinesOmitTargetFramework Include="netstandard2.0" />
    <CakeContribGuidelinesOmitTargetFramework Include="net461" />
</ItemGroup>
```
