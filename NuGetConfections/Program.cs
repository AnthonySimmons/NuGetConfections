using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGetConfections.Properties;

namespace NuGetConfections
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string repoDirectoryPath = GetRepositoryDirectoryPath(args);
                RepositoryDependencyInfo repositoryDependencyInfo = new RepositoryDependencyInfo(repoDirectoryPath);
                PackageReferenceManager packageReferenceManager = repositoryDependencyInfo.GetPackageReferences();

                IEnumerable<string> packagesWithMultipleVersions = packageReferenceManager.GetPackagesWithMultipleVersions();
                if (packagesWithMultipleVersions.Any())
                {
                    foreach (string packageIdentity in packagesWithMultipleVersions)
                    {
                        foreach (PackageReferenceInfo packageReferenceInfo in packageReferenceManager.GetPackageReferences(packageIdentity))
                        {
                            Console.WriteLine(packageReferenceInfo);
                        }
                    }
                    return 1;
                }
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                PrintUsage();
                return 2;
            }
        }

        private static string GetRepositoryDirectoryPath(string[] args)
        {
            if(args.Length == 0)
            {
                return Environment.CurrentDirectory;
            }
            else if(Directory.Exists(args[0]))
            {
                return args[0];
            }
            throw new ArgumentException(Resources.InvalidArguments);
        }

        private static void PrintUsage()
        {
            Console.WriteLine(Resources.Usage);
        }
    }
}
