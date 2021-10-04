using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class BranchType : BaseEntity
    {

        public BranchType()
        {
            BranchCollection = new List<Branch>();
        }

        public Guid BranchTypeID { get; set; }
        public string BranchName { get; set; }

        public virtual ICollection<Branch> BranchCollection { get; set; }

    }
}
