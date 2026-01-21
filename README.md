# "dnx zipsrc" command 

[![NuGet Package](https://img.shields.io/nuget/v/zipsrc.svg)](https://www.nuget.org/packages/zipsrc/) [![unit tests](https://github.com/jsakamoto/dnx-zipsrc/actions/workflows/unit-tests.yml/badge.svg)](https://github.com/jsakamoto/dnx-zipsrc/actions/workflows/unit-tests.yml) [![Discord](https://img.shields.io/discord/798312431893348414?style=flat&logo=discord&logoColor=white&label=Blazor%20Community&labelColor=5865f2&color=gray)](https://discord.com/channels/798312431893348414/1202165955900473375)

## Overview

"dnx zipsrc" is a .NET global tool that creates a zip archive from source files in your project, using .gitignore patterns to exclude artifacts, dependencies, and other non-source files. 

## System Requirements

.NET SDK Version 10.0 or later is required.

## Usage

```bash
dnx zipsrc [options]
```

Option                          | Description
------------------------------- | -----------
`-d`, `--directory <directory>` | Specifies the target directory path to create a zip archive. Default is the current directory.
`-n`, `--name <file>`           | Specifies the output zip file path. If not specified, the default name is determined automatically. For more details, see the following document.


## What gets zipped

- Recursively scans the specified directory (including subdirectories) and collects files to zip.
- Focuses on source code and other development-relevant files. Typical non-source artifacts are excluded (e.g., `bin/`, `obj/` for .NET projects, `node_modules/` for Node.js, and other build outputs, caches, or downloaded dependencies).

### Default exclusions

The following files and folders are always excluded by default:

- `.git/`, `.svn/`, `.hg/` (version control directories)
- `*.zip` files

Additionally, on Windows, files and folders with the **Hidden** attribute are excluded by default.

> [!Note]
> You can explicitly include any of these default-excluded items by specifying them in your `.gitignore` file using negation patterns (e.g., `!.git/`).

## How ignore rules are applied

- Ignore rules are based on `.gitignore` patterns.
  - If the target directory contains a `.gitignore`, its rules are honored.
  - If there is no `.gitignore` at the target directory, the tool falls back to the default rules equivalent to those from `dotnet new gitignore`.
  - `.gitignore` files in subdirectories are also read and respected. Negation patterns (e.g., `!pattern`) are correctly handled.

## Output file naming

- When `-n|--name` is specified:
  - If the provided name does not end with `.zip`, the `.zip` extension is automatically appended.
- When `-n|--name` is not specified, the output file name is decided as follows:
  1. If a `.sln` or `.slnx` file exists in the target directory, the base name of that solution file plus `.zip` is used.
  2. Otherwise, the target directory name plus `.zip` is used.
- If a file with the chosen name already exists, a numeric suffix like ` (2)`, ` (3)`, ... is appended to create a new non-conflicting name.

## Examples

- Zip the current directory using defaults:
  ```bash
  dnx zipsrc
  ```
- Zip a specific directory:
  ```bash
  dnx zipsrc -d ./src/MyApp
  ```
- Specify an explicit output name (extension auto-appended if missing):
  ```bash
  dnx zipsrc -d ./src/MyApp -n ./artifacts/MyAppSource
  # => produces ./artifacts/MyAppSource.zip
  ```

## Release Notes

See the [Releases](https://github.com/jsakamoto/dnx-zipsrc/blob/main/RELEASE-NOTES.txt)

## License

- [Mozilla Public License 2.0](https://github.com/jsakamoto/dnx-zipsrc/blob/main/LICENSE)
- Third Party Notices is [here](https://github.com/jsakamoto/dnx-zipsrc/blob/main/THIRD-PARTY-NOTICES.txt)