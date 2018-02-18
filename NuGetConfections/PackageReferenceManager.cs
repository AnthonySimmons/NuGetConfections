

using NuGetConfections;
using System.Collections.Generic;

internal class PackageReferenceManager
{
    private readonly IDictionary<string, PackageReferenceInfoForRepository> _packageReferenceInfosById = new Dictionary<string, PackageReferenceInfoForRepository>();
    
    public void Add(PackageReference packageReference, string configFileName)
    {
        if (_packageReferenceInfosById.ContainsKey(packageReference.Id))
        {
            _packageReferenceInfosById[packageReference.Id].Add(packageReference, configFileName);
        }
        else
        {
            _packageReferenceInfosById[packageReference.Id] = new PackageReferenceInfoForRepository(packageReference, configFileName);
        }
    }

    public IList<PackageReference> GetPackageReferences(string packageIdentity)
    {
        return _packageReferenceInfosById[packageIdentity].PackageReferenceInfos;
    }

    public IEnumerable<string> GetPackagesWithMultipleVersions()
    {
        IList<string> packagesWithMultipleVersions = new List<string>();
        foreach (string packageIdentity in _packageReferenceInfosById.Keys)
        {
            if (HasMultipleVersions(packageIdentity))
            {
                packagesWithMultipleVersions.Add(packageIdentity);
            }
        }
        return packagesWithMultipleVersions;
    }

    private bool HasMultipleVersions(string packageId)
    {
        return _packageReferenceInfosById[packageId].HasMultipleVersions;
    }
}