using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class itf_temp_customer
    {
        public System.Guid TransactionID { get; set; }
        public string GdateTime { get; set; }
        public string GSTT { get; set; }
        public string GDATE { get; set; }
        public string GTIME { get; set; }
        public string FSTT { get; set; }
        public Nullable<decimal> Company { get; set; }
        public string SubCust_Code { get; set; }
        public string SubCust_NameTH { get; set; }
        public string SubCust_Tel { get; set; }
        public string SubCust_Fax { get; set; }
        public string SubCust_Email { get; set; }
        public string SubCust_ContractName { get; set; }
        public Nullable<System.Guid> ContactID { get; set; }
        public Nullable<int> ContactType { get; set; }
        public Nullable<System.Guid> CreateUserID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<System.Guid> UpdateUserID { get; set; }
        public Nullable<System.DateTime> UpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
