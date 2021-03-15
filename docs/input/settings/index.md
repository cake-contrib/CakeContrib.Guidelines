---
Title: Settings
Order: 3
NoSidebar: true
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [General](#general)
  - [ProjectType](#projecttype)
- [Icons](#icons)
  - [IconOmitImport](#iconomitimport)
- [Opt-Out](#opt-out)
  - [OmitRecommendedCakeVersion](#omitrecommendedcakeversion)
  - [OmitRecommendedConfigFile](#omitrecommendedconfigfile)
  - [OmitRecommendedReference](#omitrecommendedreference)
  - [OmitRecommendedTag](#omitrecommendedtag)
  - [OmitPrivateCheck](#omitprivatecheck)
  - [OmitTargetFramework](#omittargetframework)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## General

### ProjectType
A project can be one of different types: `addin`, `module`, `recipe` or `other`. 
Some rules have different behavior for different project types.
The project type is automatically detected but can be overridden.

The rules on how project type detection is done are as follows (first matching rule applies):
* Projects not referencing `Cake.Core` or `Cake.Common` are always of type `other`
* Project names (AssemblyName, PackageId) starting with `Cake.` and ending in `.Module` will be treated as `module`
* Project names (AssemblyName, PackageId) starting with `Cake.` and ending in `.Recipe` will be treated as `recipe`
* Project names (AssemblyName, PackageId) starting with `Cake.` will be treated as `addin`
* All other projects are of type `other`

To override auto-detection use the following:

```xml
<PropertyGroup>
    <CakeContribGuidelinesProjectType>module</CakeContribGuidelinesProjectType>
</PropertyGroup>
```

:::{.alert .alert-info}
Though you can technically set `CakeContribGuidelinesProjectType` to anything you want, setting it to
different values than `addin`, `module` or `recipe` might yield unexpected results.
:::

## Icons

### IconOmitImport
<?! Include "./fragments/IconOmitImport.md" /?>

## Opt-Out

### OmitRecommendedCakeVersion
<?! Include "./fragments/OmitRecommendedCakeVersion.md" /?>

### OmitRecommendedConfigFile
<?! Include "./fragments/OmitRecommendedConfigFile.md" /?>

### OmitRecommendedReference
<?! Include "./fragments/OmitRecommendedReference.md" /?>

### OmitRecommendedTag
<?! Include "./fragments/OmitRecommendedTag.md" /?>

### OmitPrivateCheck
<?! Include "./fragments/OmitPrivateCheck.md" /?>

### OmitTargetFramework
<?! Include "./fragments/OmitTargetFramework.md" /?>
