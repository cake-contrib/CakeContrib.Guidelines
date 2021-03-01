It is possible to opt-out of the check for configuration-files by using the `CakeContribGuidelinesOmitRecommendedConfigFile` setting
and setting it's `Include` to the file name that should not be checked:

```xml
<ItemGroup>
    <CakeContribGuidelinesOmitRecommendedConfigFile Include="stylecop.json" />
    <CakeContribGuidelinesOmitRecommendedConfigFile Include=".editorconfig" />
</ItemGroup>
```