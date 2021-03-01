---
Title: Settings
Order: 3
NoSidebar: true
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [General](#general)
- [Icons](#icons)
- [Opt-Out](#opt-out)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## General

### ProjectType
A project can be one of two types: `addin` or `module`. Some rules have different behavior for 
different project types.
The project type is automatically detected. To override auto-detection use the following:

```xml
<PropertyGroup>
    <CakeContribGuidelinesProjectType>module</CakeContribGuidelinesProjectType>
</PropertyGroup>
```

:::{.alert .alert-info}
Though you can technically set `CakeContribGuidelinesProjectType` to anything you want, setting it to
different values than `addin` or `module` might yield unexpected results.
:::

## Icons

### IconDestinationLocation
<?! Include "./fragments/IconDestinationLocation.md" /?>

### IconOmitImport
<?! Include "./fragments/IconOmitImport.md" /?>

## Opt-Out

### OmitRecommendedConfigFile
<?! Include "./fragments/OmitRecommendedConfigFile.md" /?>

### OmitRecommendedReference
<?! Include "./fragments/OmitRecommendedReference.md" /?>

### OmitPrivateCheck
<?! Include "./fragments/OmitPrivateCheck.md" /?>

### OmitTargetFramework
<?! Include "./fragments/OmitTargetFramework.md" /?>
