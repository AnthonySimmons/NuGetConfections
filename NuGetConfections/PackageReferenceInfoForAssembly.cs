using NuGet.Packaging;
using NuGet.Versioning;
using System.Collections.Generic;

namespace NuGetConfections
{
    internal class PackageReferenceInfoForRepository
    {
        public List<PackageReferenceInfo> PackageReferenceInfos { get; } = new List<PackageReferenceInfo>();

        private readonly HashSet<NuGetVersion> _packageVersions = new HashSet<NuGetVersion>();

        public bool HasMultipleVersions { get; private set; }

        public PackageReferenceInfoForRepository(PackageReference packageReference, string configFileName)
        {
            AddInternal(packageReference, configFileName);
        }

        public void Add(PackageReference packageReference, string configFileName)
        {
            if (!_packageVersions.Contains(packageReference.PackageIdentity.Version))
            {
                HasMultipleVersions = true;
            }
            AddInternal(packageReference, configFileName);
        }

        private void AddInternal(PackageReference packageReference, string configFileName)
        {
            PackageReferenceInfo packageReferenceInfo = new PackageReferenceInfo(packageReference, configFileName);
            PackageReferenceInfos.Add(packageReferenceInfo);
            _packageVersions.Add(packageReferenceInfo.PackageIdentity.Version);
        }
    }
}
