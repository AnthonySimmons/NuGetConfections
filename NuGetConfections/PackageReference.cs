

namespace NuGetConfections
{
    internal class PackageReference
    {
        public PackageReference(string configFileName, string packageId, string version)
        {
            ConfigFileName = configFileName;
            Id = packageId;
            Version = version;
        }

        public string Version { get; }

        public string Id { get; }

        public string ConfigFileName { get; }

        public override string ToString()
        {
            return $"{Id} {Version}, is referenced from: '{ConfigFileName}'";
        }
    }
}
