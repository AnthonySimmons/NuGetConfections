

using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGetConfections
{
    internal class PackageReferenceInfo
    {
        private readonly PackageReference _packageReference;

        public string ConfigFileName { get; }

        public VersionRange AllowedVersions => _packageReference.AllowedVersions;

        public PackageIdentity PackageIdentity => _packageReference.PackageIdentity;

        public PackageReferenceInfo(PackageReference packageReference, string configFileName)
        {
            _packageReference = packageReference;
            ConfigFileName = configFileName;
        }

        public override string ToString()
        {
            return $"{PackageIdentity}, is referenced from: '{ConfigFileName}'";
        }
    }

}
