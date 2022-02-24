// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sparkie.InfoPlistVersioning;

public class InfoPlistUpdaterTask : Task
{
    [Required]
    public string? CFBundleVersion { get; set; }

    [Required]
    public string? CFBundleShortVersionString { get; set; }

    public override bool Execute()
    {
        if (this.CFBundleVersion is null)
        {
            Log.LogError("CFBundleVersion property is not set");
            return false;
        }

        if (this.CFBundleShortVersionString is null)
        {
            Log.LogError("CFBundleShortVersionString property is not set");
            return false;
        }

        // Get path of Info.plist
        var projectFilePath = this.BuildEngine9.ProjectFileOfTaskNode;
        var fileInfo = new FileInfo(projectFilePath);
        var rootDir = fileInfo.Directory;
        var infoPlist = Path.Combine(rootDir.FullName, "Info.plist");
        if (!File.Exists(infoPlist))
        {
            // Try MAUI Platforms directory
            infoPlist = Path.Combine(rootDir.FullName, "Platforms", "iOS", "Info.plist");
        }
        if (!File.Exists(infoPlist))
        {
            Log.LogError("Failed to find Info.plist");
            return false;
        }

        // Update the version numbers
        InfoPlistUpdater.Update(infoPlist, this.CFBundleVersion, this.CFBundleShortVersionString);

        Log.LogMessage(MessageImportance.High, $"CFBundleVersion updated to: {this.CFBundleVersion}");
        Log.LogMessage(MessageImportance.High, $"CFBundleShortVersionString updated to: {this.CFBundleShortVersionString}");
        return true;
    }
}
