using DITS.HILI.Framework;
using DITS.HILI.WMS.Core.Infrastructure.Assemblies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace DITS.HILI.WMS.WebAPIs
{
    public class CustomAssemblyResolver : IAssembliesResolver
    {
        public CustomAssemblyResolver()
        {
        }

        public ICollection<Assembly> GetAssemblies()
        {
            List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (AssembliesModel pkg in AssembliesFactory.PackageCollection)
            {
                Assembly controllersAssembly = Assembly.LoadFrom(Path.Combine(Utilities.GetCurrentDirectory(), pkg.WebAPIAssembly));
                baseAssemblies.Add(controllersAssembly);
            } 
            return baseAssemblies;
        }
    }
}