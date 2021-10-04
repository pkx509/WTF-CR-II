using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class Company : BaseEntity
    {
        public Guid CompanyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CompanyAddess> AddessCollection { get; set; }
        public virtual ICollection<Site> SiteCollection { get; set; }

        public Company()
        {
            AddessCollection = new List<CompanyAddess>();
            SiteCollection = new List<Site>();
        }
    }
}

