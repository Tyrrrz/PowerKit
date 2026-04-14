# PowerKit

[![Status](https://img.shields.io/badge/status-active-47c219.svg)](https://github.com/Tyrrrz/.github/blob/prime/docs/project-status.md)
[![Made in Ukraine](https://img.shields.io/badge/made_in-ukraine-ffd700.svg?labelColor=0057b7)](https://tyrrrz.me/ukraine)
[![Build](https://img.shields.io/github/actions/workflow/status/Tyrrrz/PowerKit/main.yml?branch=prime)](https://github.com/Tyrrrz/PowerKit/actions)
[![Coverage](https://img.shields.io/codecov/c/github/Tyrrrz/PowerKit/prime)](https://codecov.io/gh/Tyrrrz/PowerKit)
[![Version](https://img.shields.io/nuget/v/PowerKit.svg)](https://nuget.org/packages/PowerKit)
[![Downloads](https://img.shields.io/nuget/dt/PowerKit.svg)](https://nuget.org/packages/PowerKit)
[![Discord](https://img.shields.io/discord/869237470565392384?label=discord)](https://discord.gg/2SUWKFnHSm)
[![Fuck Russia](https://img.shields.io/badge/fuck-russia-e4181c.svg?labelColor=000000)](https://twitter.com/tyrrrz/status/1495972128977571848)

<table>
    <tr>
        <td width="99999" align="center">Development of this project is entirely funded by the community. <b><a href="https://tyrrrz.me/donate">Consider donating to support!</a></b></td>
    </tr>
</table>

**PowerKit** is a collection of utilities and extensions for rapid .NET development.
It's distributed as a source-only package that can be referenced without imposing any run-time dependencies.

## Terms of use<sup>[[?]](https://github.com/Tyrrrz/.github/blob/prime/docs/why-so-political.md)</sup>

By using this project or its source code, for any purpose and in any shape or form, you grant your **implicit agreement** to all the following statements:

- You **condemn Russia and its military aggression against Ukraine**
- You **recognize that Russia is an occupant that unlawfully invaded a sovereign state**
- You **support Ukraine's territorial integrity, including its claims over temporarily occupied territories of Crimea and Donbas**
- You **reject false narratives perpetuated by Russian state propaganda**

To learn more about the war and how you can help, [click here](https://tyrrrz.me/ukraine). Glory to Ukraine! 🇺🇦

## Install

- 📦 [NuGet](https://nuget.org/packages/PowerKit): `dotnet add package PowerKit`

> [!IMPORTANT]
> To reference this package, you must have the latest major version of the .NET SDK installed.
> This is only required for the build process, and does not affect which version of the runtime you can target.

## Features

- Provides utility types:
  - `Cell<T>` — a container for a value that may or may not be set (nullable-agnostic alternative to `Nullable<T>`)
  - `Disposable` — helpers for creating and composing `IDisposable` instances
  - `LockFile` — a file-based lock that prevents concurrent access to a shared resource
  - `TempFile` — a temporary file that is automatically deleted when disposed
  - `TempDirectory` — a temporary directory that is automatically deleted when disposed
- Provides extension methods for:
  - `AggregateException`, `Exception`
  - `ArrayPool<T>`
  - `Assembly`
  - `IAsyncEnumerable<T>`
  - `BinaryReader`, `BinaryWriter`
  - `bool`
  - `ICollection<T>`
  - `IComparable<T>`
  - `Console`
  - `CultureInfo`
  - `DateTime`, `DateTimeOffset`
  - `decimal`, `double`, `int`, `long`
  - `DirectoryInfo`
  - `Encoding`
  - `IEnumerable<T>`
  - `Environment`
  - `FileInfo`
  - `Guid`
  - `Path`
  - `Process`
  - `RegistryKey`
  - `Stream`
  - `StringBuilder`
  - `string`
  - `TextReader`
  - `TimeSpan`
  - `ZipArchiveEntry`
- Targets .NET Standard 2.0+, .NET Framework 3.5+, .NET 10+
- Imposes no run-time dependencies
