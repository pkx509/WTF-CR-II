using DITS.HILI.WMS.Core.CustomException;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DITS.HILI.WMS.Core.Infrastructure.Assemblies
{
    public class AssembliesFactory
    {

        private static readonly List<AssembliesModel> _packageCollection = new List<AssembliesModel>();

        public static List<AssembliesModel> PackageCollection => _packageCollection;

        public static void LoadPackages()
        {
            string packagesAsm = ConfigurationManager.AppSettings["PackagesAssemblies"];
            if (string.IsNullOrEmpty(packagesAsm))
            {
                throw new HILIException("SYS10001");
            }

            string path = Framework.Utilities.GetCurrentDirectory();
            string _file = Path.Combine(path, packagesAsm);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(AssembliesPackageCollection));
            StreamReader reader = new StreamReader(_file);
            AssembliesPackageCollection packages = (AssembliesPackageCollection)serializer.Deserialize(reader);
            if (packages.Packages == null)
            {
                return;
            }
            Dictionary<string, string> _assmfile = new Dictionary<string, string>();
            foreach (AssembliesPackage pkg in packages.Packages)
            {
                _assmfile.Add(pkg.Service, pkg.Service + ".dll");
                _assmfile.Add(pkg.Configuration, pkg.Configuration + ".dll");
                _assmfile.Add(pkg.WebApi, pkg.WebApi + ".dll");
            }
            //foreach (var item in _assmfile)
            //{
            //    if (!File.Exists(Path.Combine(path, item.Value)))
            //    {
            //        throw new HILIException(item.Value);
            //    }
            //}
            if (_assmfile.Any(item => !File.Exists(Path.Combine(path, item.Value))))
            {
                throw new HILIException("SYS10001");
            }
            foreach (AssembliesPackage pkg in packages.Packages)
            {
                _packageCollection.Add(new AssembliesModel
                {
                    Id = pkg.Id,
                    Model = pkg.Model,
                    Name = pkg.Name,
                    Configuration = pkg.Configuration,
                    ConfigurationAssembly = pkg.Configuration + ".dll",
                    Service = pkg.Service,
                    ServiceAssembly = pkg.Service + ".dll",
                    WebAPIs = pkg.WebApi,
                    WebAPIAssembly = pkg.WebApi + ".dll",
                    Version = pkg.Version,
                    ClassName = pkg.ClassName
                });
            }
            reader.Close();
        }
    }
}