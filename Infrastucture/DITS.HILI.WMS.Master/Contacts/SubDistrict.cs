namespace DITS.HILI.WMS.MasterModel.Contacts
{
    public partial class SubDistrict : BaseEntity
    {
        public System.Guid SubDistrict_Id { get; set; }
        public System.Guid District_Id { get; set; }
        public int SubDistrict_Code { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public string PostCode { get; set; }
        public bool IsActive { get; set; }
    }
}
