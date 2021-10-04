using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
{
    public class PackageCategory
    {
        public Guid PackageCategoryID { get; set; }
        public Guid PackageTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
         
        public virtual PackageType PackageType { get; set; }

        public virtual ICollection<Package> PackageCollection { get; set; }
        public PackageCategory()
        {
            PackageCollection = new List<Package>();
        }
    }
}
