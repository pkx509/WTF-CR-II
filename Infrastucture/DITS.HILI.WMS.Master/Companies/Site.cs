using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class Site : BaseEntity
    {
        public Site()
        {
            WarehouseCollection = new List<Warehouse>();
        }

        public Guid? SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteAdress { get; set; }
        public string SiteRoad { get; set; }
        public Nullable<System.Guid> SiteSubDistrict_Id { get; set; }
        public Nullable<System.Guid> SiteDistrict_Id { get; set; }
        public Nullable<System.Guid> SiteProvince_Id { get; set; }
        public string SitePostCode { get; set; }
        public string SiteCountry { get; set; }
        public string SiteTel { get; set; }
        public string SiteFax { get; set; }
        public string SiteEmail { get; set; }
        public string SiteURL { get; set; }

        public Guid? CompanyID { get; set; }


        public virtual Company Company { get; set; }
        public virtual ICollection<Warehouse> WarehouseCollection { get; set; }


    }
}
