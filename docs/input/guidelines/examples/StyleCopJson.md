---
Order: 2
Title: Example for stylecop.json
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Table of Contents](#table-of-contents)
- [Example](#example)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Example

```json
{
    "$schema": "https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json",
    "settings": {
        "indentation": {
            "indentationSize": 4,
            "tabSize": 4,
            "useTabs": false
        },
        "orderingRules": {
            "usingDirectivesPlacement": "outsideNamespace",
            "blankLinesBetweenUsingGroups": "allow",
            "systemUsingDirectivesFirst": true
        },
        "documentationRules": {
            "xmlHeader": false
        }
    }
}

```