<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->



<!-- END doctoc generated TOC please keep comment here to allow auto update -->

The cake-contrib icon will be automatically included in the project, unless
`CakeContribGuidelinesIconOmitImport` is set to `True`.

To to use a "custom" import the following could be used:

```xml
<PropertyGroup>
    <CakeContribGuidelinesIconOmitImport>True</CakeContribGuidelinesIconOmitImport>
    <PackageIcon>logo.png</PackageIcon>
</PropertyGroup>
<ItemGroup>
    <None Include="icons/logo.png" Pack="True" PackagePath="" />
</ItemGroup>
```
