<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->



<!-- END doctoc generated TOC please keep comment here to allow auto update -->

It it possible to override the detected Cake version using the `CakeContribGuidelinesOverrideTargetFrameworkCakeVersion` setting:

(*Keep in mind, though, that it is not recommended override the detected version.*)

```xml
<PropertyGroup>
    <CakeContribGuidelinesOverrideTargetFrameworkCakeVersion>2.0.0</CakeContribGuidelinesOverrideTargetFrameworkCakeVersion>
</PropertyGroup>
```