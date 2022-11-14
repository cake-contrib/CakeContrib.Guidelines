# CakeContrib.Guidelines

[![standard-readme compliant][]][standard-readme]
[![All Contributors][all-contributors-badge]](#contributors)
[![Contributor Covenant][contrib-covenantimg]][contrib-covenant]
[![Appveyor build][appveyorimage]][appveyor]
[![NuGet package][nugetimage]][nuget]

Adds common guidelines to cake-contrib projects

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Install](#install)
- [Guidelines](#guidelines)
- [Discussion](#discussion)
- [Maintainer](#maintainer)
- [Contributing](#contributing)
  - [Contributors](#contributors)
- [License](#license)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Install

use one of the following:
```ps1
Install-Package CakeContrib.Guidelines
dotnet add package CakeContrib.Guidelines
paket add CakeContrib.Guidelines

```

or add the `PackageReference` manually
```
<PackageReference Include="CakeContrib.Guidelines" Version="0.1.0" />
```

See [NuGet](https://www.nuget.org/packages/CakeContrib.Guidelines/) for the current version.

CakeContrib.Guidelines requires the use of `msbuild` version 16.8.0 or later.

## Guidelines

All [guidelines](https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines) and [rules](https://cake-contrib.github.io/CakeContrib.Guidelines/rules) are documented at <https://cake-contrib.github.io/CakeContrib.Guidelines/>

## Discussion

If you have questions, search for an existing one, or create a new discussion on the Cake GitHub repository, using the `extension-q-a` category.

[![Join in the discussion on the Cake repository](https://img.shields.io/badge/GitHub-Discussions-green?logo=github)](https://github.com/cake-build/cake/discussions)

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

CakeContrib.Guidelines follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.
Please see [the contributing file][contributing] for how to contribute to CakeContrib.Guidelines.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

This project follows the [all-contributors][] specification. Contributions of any kind welcome!

### Contributors

Thanks goes to these wonderful people ([emoji key][emoji-key]):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center"><a href="https://github.com/AdmiringWorm"><img src="https://avatars.githubusercontent.com/u/1474648?v=4?s=100" width="100px;" alt="Kim J. Nordmo"/><br /><sub><b>Kim J. Nordmo</b></sub></a><br /><a href="#question-AdmiringWorm" title="Answering Questions">💬</a> <a href="#ideas-AdmiringWorm" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center"><a href="https://github.com/Jericho"><img src="https://avatars.githubusercontent.com/u/112710?v=4?s=100" width="100px;" alt="Jericho"/><br /><sub><b>Jericho</b></sub></a><br /><a href="https://github.com/cake-contrib/CakeContrib.Guidelines/issues?q=author%3AJericho" title="Bug reports">🐛</a> <a href="#ideas-Jericho" title="Ideas, Planning, & Feedback">🤔</a></td>
      <td align="center"><a href="https://github.com/nils-a"><img src="https://avatars.githubusercontent.com/u/349188?v=4?s=100" width="100px;" alt="Nils Andresen"/><br /><sub><b>Nils Andresen</b></sub></a><br /><a href="https://github.com/cake-contrib/CakeContrib.Guidelines/commits?author=nils-a" title="Code">💻</a> <a href="#maintenance-nils-a" title="Maintenance">🚧</a></td>
      <td align="center"><a href="https://augustoproiete.net"><img src="https://avatars.githubusercontent.com/u/177608?v=4?s=100" width="100px;" alt="C. Augusto Proiete"/><br /><sub><b>C. Augusto Proiete</b></sub></a><br /><a href="#ideas-augustoproiete" title="Ideas, Planning, & Feedback">🤔</a> <a href="https://github.com/cake-contrib/CakeContrib.Guidelines/pulls?q=is%3Apr+reviewed-by%3Aaugustoproiete" title="Reviewed Pull Requests">👀</a></td>
      <td align="center"><a href="https://github.com/DiDoHH"><img src="https://avatars.githubusercontent.com/u/45682415?v=4?s=100" width="100px;" alt="DiDoHH"/><br /><sub><b>DiDoHH</b></sub></a><br /><a href="https://github.com/cake-contrib/CakeContrib.Guidelines/commits?author=DiDoHH" title="Documentation">📖</a></td>
      <td align="center"><a href="https://www.ahlgren.io"><img src="https://avatars.githubusercontent.com/u/864820?v=4?s=100" width="100px;" alt="Joel Ahlgren"/><br /><sub><b>Joel Ahlgren</b></sub></a><br /><a href="https://github.com/cake-contrib/CakeContrib.Guidelines/commits?author=squid-box" title="Code">💻</a></td>
      <td align="center"><a href="https://www.devlead.se"><img src="https://avatars.githubusercontent.com/u/1647294?v=4?s=100" width="100px;" alt="Mattias Karlsson"/><br /><sub><b>Mattias Karlsson</b></sub></a><br /><a href="https://github.com/cake-contrib/CakeContrib.Guidelines/issues?q=author%3Adevlead" title="Bug reports">🐛</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

## License

[MIT License © Nils Andresen][license]

[all-contributors]: https://github.com/all-contributors/all-contributors
[all-contributors-badge]: https://img.shields.io/github/all-contributors/cake-contrib/CakeContrib.Guidelines/develop?&style=flat-square
[appveyor]: https://ci.appveyor.com/project/cakecontrib/cakecontrib-guidelines
[appveyorimage]: https://img.shields.io/appveyor/ci/cakecontrib/cakecontrib-guidelines.svg?logo=appveyor&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/2/0/code_of_conduct/
[contrib-covenantimg]: https://img.shields.io/badge/Contributor%20Covenant-v2.0%20adopted-ff69b4.svg
[contributing]: CONTRIBUTING.md
[emoji-key]: https://allcontributors.org/docs/en/emoji-key
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/CakeContrib.Guidelines
[nugetimage]: https://img.shields.io/nuget/v/CakeContrib.Guidelines.svg?logo=nuget&style=flat-square
[license]: LICENSE.txt
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
