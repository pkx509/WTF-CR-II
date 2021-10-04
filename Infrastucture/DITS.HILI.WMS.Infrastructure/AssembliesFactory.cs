using DITS.HILI.Framework;
using DITS.HILI.WMS.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DITS.HILI.WMS.Infrastructure
{
    public class AssembliesFactory
    {

        private static List<AssembliesModel> _packageCollection = new List<AssembliesModel>();

        public static List<AssembliesModel> PackageCollection
        {
            get { return _packageCollection; }
        }
         
        public static void LoadPackages()
        {
            string packagesAsm = ConfigurationManager.AppSettings["PackagesAssemblies"];
            if (string.IsNullOrEmpty(packagesAsm))
            {
                Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Critical, MethodBase.GetCurrentMethod().Name, new Exception(MessageManger.GetMessage(Message.Core.SYS10001, packagesAsm)));
                throw new Exception(MessageManger.GetMessage(Message.Core.SYS10001, packagesAsm));
            }

            string path = DITS.HILI.Framework.Utilities.GetCurrentDirectory();
            string _file = path + "\\" + packagesAsm;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(AssembliesPackageCollection));
            StreamReader reader = new StreamReader(_file);
            var packages = (AssembliesPackageCollection)serializer.Deserialize(reader);
            if (packages.Packages == null)
                return;


            Dictionary<string, string> _assmfile = new Dictionary<string, string>();
            foreach (AssembliesPackage pkg in packages.Packages)
            {
                _assmfile.Add(pkg.Activity, pkg.Activity + ".dll");
                _assmfile.Add(pkg.Configuration, pkg.Configuration + ".dll");
                _assmfile.Add(pkg.Service, pkg.Service + ".dll");
            }
            foreach (var item in _assmfile)
            {
                FileInfo file = new FileInfo(path + "\\" + item.Value);
                if (!file.Exists)
                {
                    Logging.Log(MethodBase.GetCurrentMethod().DeclaringType, LogLevelEnum.Critical, MethodBase.GetCurrentMethod().Name,
                        new Exception(MessageManger.GetMessage(Message.Core.SYS10002, item.Value)));
                    throw new Exception(MessageManger.GetMessage(Message.Core.SYS10001, item.Value));
                }
            }

            foreach (AssembliesPackage pkg in packages.Packages)
            {
                _packageCollection.Add(new AssembliesModel
                {
                    Id = pkg.Id,
                    Activity = pkg.Activity,
                    ActivityAssembly = pkg.Activity + ".dll",
                    Configuration = pkg.Configuration,
                    ConfigurationAssembly = pkg.Configuration + ".dll",
                    Service = pkg.Service,
                    ServiceAssembly = pkg.Service + ".dll",
                    Version = pkg.Version
                });
            }

            reader.Close();
        }

    }

}
