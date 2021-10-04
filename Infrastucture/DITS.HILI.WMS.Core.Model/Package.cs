using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.PackagesModel
{
    public class Package
    {
        public Guid PackageID { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        public Guid PackageCategoryID { get; set; }

        public virtual PackageCategory PackageCategory { get; set; }

        public virtual ICollection<WorkFlow> WorkFlowSourceCollection { get; set; }
        public virtual ICollection<WorkFlow> WorkFlowDestinationCollection { get; set; }
        public virtual ICollection<PackageWorkFlow> PackageWorkFlowCollection { get; set; }
        public Package()
        {
            WorkFlowSourceCollection = new List<WorkFlow>();
            WorkFlowDestinationCollection = new List<WorkFlow>();
            PackageWorkFlowCollection = new List<PackageWorkFlow>();
        }
    }
}
