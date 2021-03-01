<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->



<!-- END doctoc generated TOC please keep comment here to allow auto update -->

The default location of the cake-contrib icon is `icon.png`, next to the csproj (i.e. `$(MSBuildProjectDirectory)/icon.png`).

Setting `CakeContribGuidelinesIconDestinationLocation` makes it possible to override the default location of the Icon. For example setting 

```xml
<PropertyGroup>
    <CakeContribGuidelinesIconDestinationLocation>../logo.png</CakeContribGuidelinesIconDestinationLocation>
</PropertyGroup>
```

in the csproj will place the icon as `logo.png` one folder up (relative to the current project).