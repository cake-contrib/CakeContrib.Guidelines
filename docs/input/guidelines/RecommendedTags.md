---
Title: Recommended Tags
---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Goals](#goals)
  - [Recommended tags](#recommended-tags)
  - [Tag delimiter](#tag-delimiter)
- [Related rules](#related-rules)
- [Usage](#usage)
- [Settings](#settings)
  - [Opt-Out](#opt-out)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Goals

To have consistency and better discoverability, also to have the correct installation instructions
displayed in the NuGet Gallery (compare this [NuGet proposal](https://github.com/NuGet/NuGetGallery/issues/8381)) some tags are recommended for a NuGet package.

<!-- TODO: We should link the cakebuild.net - blog post, when that feature is released!! -->

### Recommended tags

Depending on the package type, different tags are suggested:

* for every package:
  * `cake`
  * `script`
  * `build`
  * `cake-build`
* additionally, for an `addin`:
  * `addin`
  * `cake-addin`
* additionally, for a `module`:
  * `module`
  * `cake-module`
* additionally, for a `recipe`:
  * `recipe`
  * `cake-recipe`

Additionally, for addins, it is always advised to include an additional tag to describe the
addin further. (E.g. add `twitter` tag to the `Cake.Twitter` addin.)

### Tag delimiter

NuGet specifications require the delimiter of tags to be a space. MsBuild supports the semicolon `;`
as an additional delimiter (effectively converting all semicolons to spaces during packaging.)

While having comma (`,`) as a delimiter currently seems to work in the NuGet Gallery, it is not
a supported delimiter for tags.

## Related rules

 * [CCG0008](../rules/ccg0008)

These rules are only applied for [project types](../settings#projecttype) `addin`, `module` and `recipe`.

## Usage

Using this package automatically enables this guideline.

## Settings

### Opt-Out

<?! Include "../settings/fragments/OmitRecommendedTag.md" /?>
