using System;

namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public partial class Province : BaseEntity
    {
        public System.Guid Province_Id { get; set; }
        public Nullable<int> Region_Id { get; set; }
        public Nullable<int> Country_Id { get; set; }
        public string Province_Code { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }

        public bool IsActive { get; set; }
    }
}
