using System;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class CompanyAddess : BaseAddress
    {
        public Guid CompanyID { get; set; }

        public virtual Company Company { get; set; }
        public CompanyAddess()
        { }
    }
}
