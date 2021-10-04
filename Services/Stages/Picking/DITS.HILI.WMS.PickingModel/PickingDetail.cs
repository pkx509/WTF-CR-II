using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PickingModel
{
    public class PickingDetail : BaseEntity
    {
        public Guid PickingDetailID { get; set; }
        public Guid? AssignID { get; set; }
        public Guid? PickStockUnitID { get; set; }
        public decimal? PickStockQty { get; set; }
        public Guid? PickBaseUnitID { get; set; }
        public decimal? PickBaseQty { get; set; }
        public decimal? ConversionQty { get; set; }
        public int? PickingStatus { get; set; }
        public string PickingReason { get; set; }
        public Guid? LocationID { get; set; }
        public string PalletCode { get; set; }

        public virtual PickingAssign Assign { get; set; }

        [NotMapped]
        public virtual Product Product { get; set; }
        [NotMapped]
        public virtual ProductUnit PickStockUnit { get; set; }
        [NotMapped]
        public virtual ProductUnit PickBaseUnit { get; set; }
        [NotMapped]
        public virtual Location Location { get; set; }

    }
}
