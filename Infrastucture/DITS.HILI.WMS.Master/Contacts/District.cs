using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public partial class District : BaseEntity
    {
        public System.Guid District_Id { get; set; }
        public System.Guid Province_Id { get; set; }
        public string District_Code { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public string PostCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
