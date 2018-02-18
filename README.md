# NuGet Confections

Extends the [NuGet.Client API](https://github.com/NuGet/NuGet.Client) to provide some additional tools to manage package dependencies.

# Availability
This tool is available as a NuGet package, and can be downloaded at [NuGet.org](https://www.nuget.org/packages/NuGetConfections/).


## Package Consolidation
This tool provides the ability to verify that referenced packages use the same versions across an entire repository, via command line.
Every packages.config file within the repository directory is included in the verification.

## Usage

```
NuGet Confections
Provides tools to validate and manage NuGet package references.

Usage:
NuGetConfections.exe <Action> [<Options>]

Actions:
VerifyConsolidation - Scans the repository and verifies that all package versions have been consolidated. 
Returns an exit code of 2, when multiple versions of the same package are referenced. The repository directory may be provided via command line, defaults to the current directory. Every packages.config file located in the repository directory is included in the verification.

Examples:
NuGetConfections.exe VerifyConsolidation
NuGetConfections.exe VerifyConsolidation C:\RepoPath
```