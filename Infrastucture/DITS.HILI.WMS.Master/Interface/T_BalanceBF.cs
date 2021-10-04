namespace DITS.HILI.WMS.MasterModel.Interface
{
    public partial class T_BalanceBF
    {
        public System.DateTime BFBalanceDT { get; set; }
        public System.Guid ProductID { get; set; }
        public decimal BaseQuantity { get; set; }
        public System.Guid BaseUnitID { get; set; }
        public decimal StockQuantity { get; set; }
        public System.Guid StockUnitID { get; set; }
    }
}
