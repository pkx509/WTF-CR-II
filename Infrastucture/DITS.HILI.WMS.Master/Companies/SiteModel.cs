using System;

namespace DITS.HILI.WMS.MasterModel.Companies
{
    public class SiteModel
    {
        public Guid? SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteAdress { get; set; }
        public string SiteRoad { get; set; }
        public Guid? SubDistrict_Id { get; set; }
        public string SubDistrictName { get; set; }
        public Guid? District_Id { get; set; }
        public string DistrictName { get; set; }
        public Guid? Province_Id { get; set; }
        public string ProvinceName { get; set; }
        public string SitePostCode { get; set; }
        public string SiteCountry { get; set; }
        public string SiteTel { get; set; }
        public string SiteFax { get; set; }
        public string SiteEmail { get; set; }
        public string SiteURL { get; set; }
        public bool IsActive { get; set; }

        public Guid? CompanyID { get; set; }


    }
}
