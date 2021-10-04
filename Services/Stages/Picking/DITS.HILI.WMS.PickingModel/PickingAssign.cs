using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.HILI.WMS.ProductionControlModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PickingModel
{
    public class PickingAssign : BaseEntity
    {
        public PickingAssign()
        {
            PickingDetailCollection = new List<PickingDetail>();
        }

        public Guid AssignID { get; set; }
        public System.Guid ShippingDetailID { get; set; }
        public Guid? PickingID { get; set; }
        public Guid? ProductID { get; set; }
        public decimal? BaseQuantity { get; set; }
        public Guid? BaseUnitID { get; set; }
        public string Barcode { get; set; }
        public Guid? StockUnitID { get; set; }
        public decimal? StockQuantity { get; set; }
        public Guid? RefLocationID { get; set; }
        public Guid? SuggestionLocationID { get; set; }
        public Guid? PalletUnitID { get; set; }
        public string RefPalletCode { get; set; }
        public string PalletCode { get; set; }
        public decimal? PalletQty { get; set; }
        public string PickingLot { get; set; }
        public Guid? PickingUserID { get; set; }
        public DateTime? PickingDate { get; set; }
        public int? OrderPick { get; set; }
        public PickingStatusEnum AssignStatus { get; set; }

        public Guid? BookingID { get; set; }

        public virtual Picking Picking { get; set; }

        public virtual ICollection<PickingDetail> PickingDetailCollection { get; set; }

        [NotMapped]
        public virtual ProductionControlDetail PalletUnit { get; set; }
        [NotMapped]
        public virtual Product Product { get; set; }
        [NotMapped]
        public virtual ProductUnit BaseUnit { get; set; }
        [NotMapped]
        public virtual ProductUnit StockUnit { get; set; }
        [NotMapped]
        public virtual Location SuggestionLocation { get; set; }
    }
}
