using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
{
    public class PackageType
    {
        public Guid PackageTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<PackageCategory> PackageCategoryCollection { get; set; }

        public PackageType()
        {
            PackageCategoryCollection = new List<PackageCategory>();
        }

    }
}
