

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace NuGetConfections
{
    internal class PackageConfig
    {
        private List<PackageReference> _packageReferences = new List<PackageReference>();
        public IReadOnlyCollection<PackageReference> PackageReferences { get; }

        private readonly string _packageConfigFileName;

        public PackageConfig(string packageConfigFileName)
        {
            _packageConfigFileName = packageConfigFileName;
            PackageReferences = new ReadOnlyCollection<PackageReference>(_packageReferences);
        }

        public void Load()
        {
            using (FileStream stream = File.Open(_packageConfigFileName, FileMode.Open))
            {
                Load(stream);
            }
        }

        public void Load(Stream packageConfigFileStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(packageConfigFileStream);
            Load(doc);
        }

        public void Load(XmlDocument xmlDoc)
        {
            foreach(XmlNode node in xmlDoc.GetElementsByTagName("package"))
            {
                string id = node.Attributes["id"]?.Value;
                string version = node.Attributes["version"]?.Value;

                _packageReferences.Add(new PackageReference(_packageConfigFileName, id, version));
            }
        }
    }
}
