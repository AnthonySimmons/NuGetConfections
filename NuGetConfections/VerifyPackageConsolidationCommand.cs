

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGetConfections.Properties;

namespace NuGetConfections
{
    internal class VerifyPackageConsolidationCommand : INuGetConfectionCommand
    {
        private readonly string _directoryPath;

        public VerifyPackageConsolidationCommand(CommandOptions commandOptions)
        {
            _directoryPath = GetRepositoryDirectoryPath(commandOptions.Options);
        }

        public bool TryRun(out string errorString)
        {
            bool success = true;
            errorString = null;
            RepositoryDependencyInfo repositoryDependencyInfo = new RepositoryDependencyInfo(_directoryPath);
            PackageReferenceManager packageReferenceManager = repositoryDependencyInfo.GetPackageReferences();

            IEnumerable<string> packagesWithMultipleVersions = packageReferenceManager.GetPackagesWithMultipleVersions();
            if (packagesWithMultipleVersions.Any())
            {
                errorString = Resources.UnconsolidatedPackageVersionsFound + Environment.NewLine;
                foreach (string packageIdentity in packagesWithMultipleVersions)
                {
                    foreach (PackageReferenceInfo packageReferenceInfo in packageReferenceManager.GetPackageReferences(packageIdentity))
                    {
                        errorString += $"{packageReferenceInfo}{Environment.NewLine}";
                    }
                }
                success = false;
            }
            return success;
        }

        private static string GetRepositoryDirectoryPath(IEnumerable<string> options)
        {
            if (!options.Any())
            {
                return Environment.CurrentDirectory;
            }
            else if (Directory.Exists(options.First()))
            {
                return options.First();
            }
            throw new ArgumentException(Resources.InvalidArguments);
        }
    }
}
