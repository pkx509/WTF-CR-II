using System;

namespace DITS.HILI.WMS.MasterModel.Interface
{

    public partial class itf_temp_product_unit
    {
        public System.Guid ProductUnitID { get; set; }
        public string Product_system_code { get; set; }
        public Nullable<decimal> UnitSeq { get; set; }
        public string AlternateUnit { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Conversion { get; set; }
        public string BasicUnit { get; set; }
        public string GDATE { get; set; }
        public string GTIME { get; set; }
        public string GSTT { get; set; }
        public string FDATE { get; set; }
        public string FTIME { get; set; }
        public string FSTT { get; set; }
        public Nullable<System.Guid> CreateUserID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<System.Guid> UpdateUserID { get; set; }
        public Nullable<System.DateTime> UpdateDateTime { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public Nullable<bool> IsBaseUOM { get; set; }
        public Nullable<int> StandardPallet { get; set; }
    }
}
