# Sparkie.InfoPlistVersioning

[![NuGet Package](https://img.shields.io/nuget/v/Sparkie.InfoPlistVersioning.svg)](https://www.nuget.org/packages/Sparkie.InfoPlistVersioning)

An MSBuild Task to update Info.plist version entries.

## InfoPlistUpdaterTask

The nuget contains a single task named "InfoPlistUpdaterTask" with two required properties:
| Property                  | Value                                                                          |
| ------------------------- | ------------------------------------------------------------------------------ |
|CFBundleVersion | The value to insert under the CFBundleVersion key in the Info.plist |
|CFBundleShortVersionString | The value to insert under the CFBundleShortVersionString key in the Info.plist |

The task will try to locate the Info.plist in either:
- the same directory as the .csproj file
- the Platforms/iOS directory for use with MAUI

Note that both the CFBundleVersion and CFBundleShortVersionString keys must already be present in the Info.plist file, i.e. the MSBuild task only updates existing entries, it does not add the keys if not already present.

## How to use with [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning)

Set up Nerdbank.GitVersioning, as [described here](https://github.com/dotnet/Nerdbank.GitVersioning#readme).

Make sure you have:
- added the Nerdbank.GitVersioning package to your iOS project
- added a version.json to your iOS project

Add the Sparkie.InfoPlistVersioning package to your project either via Visual Studio nuget manager or the CLI:

```
dotnet add package Sparkie.InfoPlistVersioning
```

Add the following entry to the iOS app project file:

```xml
  <Target Name="InfoPlistUpdaterTask" AfterTargets="GetBuildVersion">
    <InfoPlistUpdaterTask CFBundleVersion="$(BuildVersion3Components)" CFBundleShortVersionString="$(BuildVersion3Components)" />
  </Target>
```

Make sure the task executes after the Nerdbank.GitVersioning GetBuildVersion task otherwise the BuildVersion3Components property will not be set.
