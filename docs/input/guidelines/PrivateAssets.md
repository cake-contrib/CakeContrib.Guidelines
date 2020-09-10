---
Order: 2
Title: PrivateAssets in references
---

- [Goals](#Goals)
- [Related rules](#Related-rules)
- [Settings](#Settings)
  - [Opt-Out](#Opt-Out)

## Goals

As the recommendation from upstream is always to have `Cake.Core` and `Cake.Common` set as private assets, this is checked on build.

## Related rules

 * [CCG0004](../rules/ccg0004)

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

It it possible to opt-out of the check for `PrivateAssets` using the following setting:

(*Keep in mind, though that it is not recommended to opt-out of this feature*)

```xml
<PropertyGroup>
    <CakeContribGuidelinesCakeReferenceOmitPrivateCheck>1</CakeContribGuidelinesCakeReferenceOmitPrivateCheck>
</PropertyGroup>
```
