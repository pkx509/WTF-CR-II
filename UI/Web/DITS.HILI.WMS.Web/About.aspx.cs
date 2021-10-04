using DITS.HILI.WMS.ClientService;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace DITS.HILI.WMS.Web
{
    public partial class About : BaseUIPage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            System.Collections.Generic.List<AssembliesModel> packages = await WMSProperty.GetSystem();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            string guid = "";
            object[] objects = assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            if (objects.Length > 0)
            {
                guid = ((System.Runtime.InteropServices.GuidAttribute)objects[0]).Value;
            }

            bool ok = packages.Any(s => s.Id == guid);
            if (!ok)
            {
                AssembliesModel pkg = new AssembliesModel
                {
                    Id = assembly.GetType().GUID.ToString(),
                    Name = fvi.ProductName,
                    Version = version
                };

                packages.Add(pkg);
            }
            GridView1.DataSource = packages.ToList();
            GridView1.DataBind();
        }
    }
}