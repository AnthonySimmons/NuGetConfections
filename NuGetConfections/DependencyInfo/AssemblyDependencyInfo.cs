
using NuGetConfections.PackageReferences;
using System.Collections.Generic;

namespace NuGetConfections.DependencyInfo
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
            PackageConfig packageConfig = new PackageConfig(ConfigFileName);
            packageConfig.Load();
            return packageConfig.PackageReferences;
        }
    }
}