using System;

namespace DITS.HILI.WMS.Core.Infrastructure.Assemblies
{

    [Serializable()]
    [System.Xml.Serialization.XmlTypeAttribute()]
    public class AssembliesPackage
    {
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("model")]
        public string Model { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("configuration")]
        public string Configuration { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("service")]
        public string Service { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("webapi")]
        public string WebApi { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("version")]
        public string Version { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("class")]
        public string ClassName { get; set; }

    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("packages")]
    public class AssembliesPackageCollection
    {
        [System.Xml.Serialization.XmlElementAttribute("package")]
        public AssembliesPackage[] Packages { get; set; }
    }

    public class AssembliesModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Service { get; set; }
        public string ClassName { get; set; }
        public string Configuration { get; set; }
        public string WebAPIs { get; set; }
        public string ConfigurationAssembly { get; set; }
        public string ServiceAssembly { get; set; }
        public string WebAPIAssembly { get; set; }
        public string Version { get; set; }
    }


}
