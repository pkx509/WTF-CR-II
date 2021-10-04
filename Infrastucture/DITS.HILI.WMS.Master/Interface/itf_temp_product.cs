using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{

    public partial class itf_temp_product
    {
        public System.Guid TransactionID { get; set; }
        public string Product_system_code { get; set; }
        public string Product_Name_Full { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Shelf_life { get; set; }
        public Nullable<int> ProductAge { get; set; }
        public string ProductBasicUnit { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductGroup { get; set; }
        public string ProductType { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public string TransactionState { get; set; }
        public string FSTT { get; set; }
        public string FDATE { get; set; }
        public string FTIME { get; set; }
        public string GSTT { get; set; }
        public string GDATE { get; set; }
        public string GTIME { get; set; }
        public string STAS { get; set; }
        public string ErrorMessage { get; set; }
    }
}
