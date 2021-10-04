using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Infrastructure
{

    [Serializable()]
    [System.Xml.Serialization.XmlTypeAttribute()]
    public class AssembliesPackage
    {
        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string Id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("activity")]
        public string Activity { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("configuration")]
        public string Configuration { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("service")]
        public string Service { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("version")]
        public string Version { get; set; }

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
        public string Activity { get; set; }
        public string Configuration { get; set; }
        public string Service { get; set; }
        public string ActivityAssembly { get; set; }
        public string ConfigurationAssembly { get; set; }
        public string ServiceAssembly { get; set; }
        public string Version { get; set; }

    }
}
