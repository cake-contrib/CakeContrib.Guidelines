The cake-contrib icon will be automatically included in the project, unless `CakeContribGuidelinesIconOmitImport` was defined.

To to use a "custom" import the following could be used:

```xml
<PropertyGroup>
    <CakeContribGuidelinesIconOmitImport>1</CakeContribGuidelinesIconOmitImport>
</PropertyGroup>
<ItemGroup>
    <None Include="$(CakeContribGuidelinesIconDestinationLocation)">
        <Pack>True</Pack>
        <PackagePath></PackagePath>
    </None>
</ItemGroup> 
```
