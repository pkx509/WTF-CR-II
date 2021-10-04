using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.PackagesModel
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
