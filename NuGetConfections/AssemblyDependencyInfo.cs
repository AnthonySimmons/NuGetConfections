
using NuGet.Packaging;
using System.Collections.Generic;
using System.IO;

namespace NuGetConfections
{
    internal class AssemblyDependencyInfo
    {
        public string ConfigFileName { get; }

        public AssemblyDependencyInfo(string configFileName)
        {
            ConfigFileName = configFileName;
        }

        public IEnumerable<PackageReference> GetPackageReferences()
        {
            using (Stream configFileStream = File.Open(ConfigFileName, FileMode.Open))
            {
                PackagesConfigReader packagesConfigReader = new PackagesConfigReader(configFileStream);
                return packagesConfigReader.GetPackages();
            }
        }
    }
}
