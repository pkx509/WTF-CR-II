using System;

namespace DITS.HILI.WMS.MasterModel
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BaseAddress : BaseEntity
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public BaseAddress()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int Sequence { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        public string Address { get; set; }
        public Guid SubDistrict_Id { get; set; }
        public Guid District_Id { get; set; }
        public Guid Province_Id { get; set; }
        public string PostCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
    }
}
