<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->



<!-- END doctoc generated TOC please keep comment here to allow auto update -->

It it possible to opt-out of the check for recommended tags by using the `CakeContribGuidelinesOmitRecommendedTag` setting
and setting it's `Include` to the tag that should not be checked.

(*Keep in mind, though, that it is not recommended to opt-out of this feature*)

```xml
<ItemGroup>
    <CakeContribGuidelinesOmitRecommendedTag Include="script" />
    <CakeContribGuidelinesOmitRecommendedTag Include="build" />
</ItemGroup>
```