---
Title: Target Frameworks
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Goals](#goals)
  - [Provided packages](#provided-packages)
    - [Cake v1.0](#cake-v10)
    - [Cake v2.0.0](#cake-v200)
    - [Cake v3.0.0](#cake-v300)
- [Related rules](#related-rules)
- [Usage](#usage)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

When an addin/module references a package that is also provided by Cake at runtime,
the version the addin/module references should match the version provided by Cake.
Also, the reference of the package in the addin/module should be set as private assets.

### Provided packages

#### Cake v1.0

| Reference                                | Version       |
| ---------------------------------------- | ------------- |
| Autofac                                  | 6.1.0         |
| Microsoft.CodeAnalysis.CSharp.Scripting  | 3.9.0-1.final |
| Microsoft.CSharp                         | 4.7.0         |
| Microsoft.DotNet.PlatformAbstractions    | 3.1.6         |
| Microsoft.Extensions.DependencyInjection | 5.0.1         |
| Microsoft.NETCore.Platforms              | 5.0.0         |
| Microsoft.Win32.Registry                 | 5.0.0         |
| Newtonsoft.Json                          | 12.0.3        |
| NuGet.Common                             | 5.8.0         |
| NuGet.Frameworks                         | 5.8.0         |
| NuGet.Packaging                          | 5.8.0         |
| NuGet.Protocol                           | 5.8.0         |
| NuGet.Resolver                           | 5.8.0         |
| NuGet.Versioning                         | 5.8.0         |
| System.Collections.Immutable             | 5.0.0         |
| System.Reflection.Metadata               | 5.0.0         |
| xunit                                    | 2.4.1         |

#### Cake v2.0.0

| Reference                                | Version |
| ---------------------------------------- | ------- |
| Autofac                                  | 6.3.0   |
| Microsoft.CodeAnalysis.CSharp.Scripting  | 4.0.1   |
| Microsoft.CSharp                         | 4.7.0   |
| Microsoft.DotNet.PlatformAbstractions    | 3.1.6   |
| Microsoft.Extensions.DependencyInjection | 6.0.0   |
| Microsoft.NETCore.Platforms              | 6.0.0   |
| Microsoft.Win32.Registry                 | 5.0.0   |
| Newtonsoft.Json                          | 13.0.1  |
| NuGet.Common                             | 5.11.0  |
| NuGet.Frameworks                         | 5.11.0  |
| NuGet.Packaging                          | 5.11.0  |
| NuGet.Protocol                           | 5.11.0  |
| NuGet.Resolver                           | 5.11.0  |
| NuGet.Versioning                         | 5.11.0  |
| System.Collections.Immutable             | 6.0.0   |
| System.Reflection.Metadata               | 6.0.0   |
| xunit                                    | 2.4.1   |

#### Cake v3.0.0

| Reference                                | Version       |
| ---------------------------------------- | ------------- |
| Autofac                                  | 6.4.0         |
| Microsoft.CodeAnalysis.CSharp.Scripting  | 4.4.0-4.final |
| Microsoft.CSharp                         | 4.7.0         |
| Microsoft.Extensions.DependencyInjection | 7.0.0         |
| Microsoft.NETCore.Platforms              | 7.0.0         |
| Microsoft.Win32.Registry                 | 5.0.0         |
| Newtonsoft.Json                          | 13.0.1        |
| NuGet.Common                             | 6.3.1         |
| NuGet.Frameworks                         | 6.3.1         |
| NuGet.Packaging                          | 6.3.1         |
| NuGet.Protocol                           | 6.3.1         |
| NuGet.Resolver                           | 6.3.1         |
| NuGet.Versioning                         | 6.3.1         |
| System.Collections.Immutable             | 7.0.0         |
| System.Reflection.Metadata               | 7.0.0         |
| xunit                                    | 2.4.2         |

## Related rules

 * [CCG0010](../rules/ccg0010)

These rules are only applied for [project types](../settings#projecttype) `addin` and `module`.

## Usage

Using this package automatically enables this guideline.
