

using System;
using System.Collections.Generic;
using System.Linq;
using NuGetConfections.DependencyInfo;
using NuGetConfections.PackageReferences;
using NuGetConfections.Properties;

namespace NuGetConfections.Commands
{
    internal class VerifyPackageConsolidationCommand : INuGetConfectionCommand
    {
        private readonly string _directoryPath;

        private readonly IEnumerable<string> _ignoredPackages;

        public VerifyPackageConsolidationCommand(CommandOptions commandOptions)
        {
            _directoryPath = GetRepositoryDirectoryPath(commandOptions.Options);
            _ignoredPackages = GetIgnoredPackages(commandOptions.Options);
        }

        public bool TryRun(out string outputMessage)
        {
            bool success = true;
            outputMessage = Resources.AllPackagesConsolidated;
            RepositoryDependencyInfo repositoryDependencyInfo = new RepositoryDependencyInfo(_directoryPath);
            PackageReferenceManager packageReferenceManager = repositoryDependencyInfo.GetPackageReferences();

            if (!packageReferenceManager.HasPackageReferences())
            {
                outputMessage = Resources.NoPackageReferencesFound;
            }
            else
            {
                IEnumerable<string> packagesWithMultipleVersions = packageReferenceManager.GetPackagesWithMultipleVersions();
                IEnumerable<string> nonIgnoredPackagesWithMultipleVersions = packagesWithMultipleVersions.Except(_ignoredPackages);
                IEnumerable<string> ignoredPackagesWithMultipleVersions = packagesWithMultipleVersions.Intersect(_ignoredPackages);

                if (ignoredPackagesWithMultipleVersions.Any())
                {
                    outputMessage = string.Join(Environment.NewLine, ignoredPackagesWithMultipleVersions.Select(package => string.Format(Resources.Package_0_IsIgnoredAndHasMultipleVersions, package)));
                }

                if (nonIgnoredPackagesWithMultipleVersions.Any())
                {
                    outputMessage = Resources.UnconsolidatedPackageVersionsFound + Environment.NewLine;
                    foreach (string packageIdentity in nonIgnoredPackagesWithMultipleVersions)
                    {
                        foreach (PackageReference packageReferenceInfo in packageReferenceManager.GetPackageReferences(packageIdentity))
                        {
                            outputMessage += $"{packageReferenceInfo}{Environment.NewLine}";
                        }
                    }
                    success = false;
                }
            }
            return success;
        }

        private static string GetRepositoryDirectoryPath(IEnumerable<string> options)
        {
            return GetArgValue(options, "repository") ?? Environment.CurrentDirectory;
        }

        private static IList<string> GetIgnoredPackages(IEnumerable<string> options)
        {
            //NuGetConfections.exe VerifyConsolidation Ignored="NUnit,AutoFixture"
            //NuGetConfections.exe VerifyConsolidation repository=C:\RepoPath Ignored="NUnit,AutoFixture"
            List<string> ignoredPackages = new List<string>();

            string argValue = GetArgValue(options, "ignored");
            if(argValue != null)
            {
                ignoredPackages = argValue.Split(',').ToList();
            }

            return ignoredPackages;
        }


        private static string GetArgValue(IEnumerable<string> options, string argName)
        {
            List<string> ignoredPackages = new List<string>();
            foreach (string option in options)
            {
                if (option.ToLower().StartsWith(argName))
                {
                    string[] optionSegments = option.Split('=');
                    return optionSegments[1];
                }
            }
            return null;
        }
    }
}
