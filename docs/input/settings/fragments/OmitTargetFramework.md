<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->



<!-- END doctoc generated TOC please keep comment here to allow auto update -->

It it possible to opt-out of the check for target framework(s) by using the `CakeContribGuidelinesOmitTargetFramework` setting
and setting it's `Include` to the target framework that should not be checked.

(*Keep in mind, though, that it is not recommended to opt-out of this feature*)

```xml
<ItemGroup>
    <CakeContribGuidelinesOmitTargetFramework Include="netstandard2.0" />
    <CakeContribGuidelinesOmitTargetFramework Include="net461" />
</ItemGroup>
```

