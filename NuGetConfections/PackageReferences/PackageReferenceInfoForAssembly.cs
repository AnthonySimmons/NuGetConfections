
using System.Collections.Generic;

namespace NuGetConfections.PackageReferences
{
    internal class PackageReferenceInfoForRepository
    {
        public List<PackageReference> PackageReferenceInfos { get; } = new List<PackageReference>();

        private readonly HashSet<string> _packageVersions = new HashSet<string>();

        public bool HasMultipleVersions { get; private set; }

        public PackageReferenceInfoForRepository(PackageReference packageReference, string configFileName)
        {
            AddInternal(packageReference, configFileName);
        }

        public void Add(PackageReference packageReference, string configFileName)
        {
            if (!_packageVersions.Contains(packageReference.Version))
            {
                HasMultipleVersions = true;
            }
            AddInternal(packageReference, configFileName);
        }

        private void AddInternal(PackageReference packageReference, string configFileName)
        {
            PackageReference packageReferenceInfo = new PackageReference(configFileName, packageReference.Id, packageReference.Version);
            PackageReferenceInfos.Add(packageReferenceInfo);
            _packageVersions.Add(packageReferenceInfo.Version);
        }
    }
}
