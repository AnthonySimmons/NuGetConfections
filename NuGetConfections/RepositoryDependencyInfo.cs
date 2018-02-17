using NuGet.Packaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace NuGetConfections
{
    internal class RepositoryDependencyInfo
    {
        private readonly string mRepoDirectoryPath;

        private readonly IList<AssemblyDependencyInfo> _assemblyDependencyInfos = new List<AssemblyDependencyInfo>();

        public IReadOnlyCollection<AssemblyDependencyInfo> AssemblyDependencyInfos;
        
        public RepositoryDependencyInfo(string repoDirectoryPath)
        {
            mRepoDirectoryPath = repoDirectoryPath;
            AssemblyDependencyInfos = new ReadOnlyCollection<AssemblyDependencyInfo>(_assemblyDependencyInfos);
        }

        private void LoadAssemblies()
        {
            string[] packageConfigFiles = Directory.GetFiles(mRepoDirectoryPath, "packages.config", SearchOption.AllDirectories);
            foreach(string packageConfigFile in packageConfigFiles)
            {
                _assemblyDependencyInfos.Add(new AssemblyDependencyInfo(packageConfigFile));
            }
        }

        private PackageReferenceManager LoadDependencyInfo()
        {
            PackageReferenceManager packageReferenceManager = new PackageReferenceManager();
            foreach(AssemblyDependencyInfo assemblyDependencyInfo in _assemblyDependencyInfos)
            {
                foreach(PackageReference packageReference in assemblyDependencyInfo.GetPackageReferences())
                {
                    packageReferenceManager.Add(packageReference, assemblyDependencyInfo.ConfigFileName);
                }
            }
            return packageReferenceManager;
        }
        
        public PackageReferenceManager GetPackageReferences()
        {
            LoadAssemblies();
            return LoadDependencyInfo();
        }
    }
}
