using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

namespace DITS.HILI.WMS.WebAPIs.Controllers
{
    public class HILIServiceController : ApiController
    {

        public async Task<IHttpActionResult> GetSystem()
        {

            List<AssembliesModel> packages = AssembliesFactory.PackageCollection.Where(x => x.Id != "b3958bc6-781f-429d-99fa-2bc81a157c54").ToList();
            List<AssembliesModel> pcks = new List<AssembliesModel>();
            Assembly assembly;
            foreach (AssembliesModel pkg in packages)
            {
                assembly = Assembly.LoadFrom(string.Format("{0}\\{1}", Utilities.GetCurrentDirectory(), pkg.ServiceAssembly));
                pkg.Version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                pcks.Add(pkg);
            }

            assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string guid = "";
            object[] objects = assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            if (objects.Length > 0)
            {
                guid = ((System.Runtime.InteropServices.GuidAttribute)objects[0]).Value;
            }

            bool ok = AssembliesFactory.PackageCollection.Any(s => s.Id == guid);
            if (!ok)
            {
                AssembliesModel pkg = new AssembliesModel
                {
                    Version = version,
                    Name = fvi.ProductName,
                    Id = guid,
                };
                pcks.Add(pkg);
            }

            return Ok(pcks.ToList());
        }
    }
}
