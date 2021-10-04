using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class Changestatus : BaseEntity
    {

        public Changestatus()
        {

        }

        public Guid DamageID { get; set; }
        public string DamageCode { get; set; }
        public Guid ProductID { get; set; }
        public string PalletCode { get; set; }
        public string ReprocessPalletCode { get; set; }
        public string RejectPalletCode { get; set; }
        public string ReprocessLot { get; set; }
        public string RejectLot { get; set; }
        public Guid BaseUnitID { get; set; }
        public Guid StockUnitID { get; set; }
        public Guid DocumentID { get; set; }
        public InspectionStatus InspectionStatus { get; set; }
        public Guid LineID { get; set; }
        public Guid ReasonID { get; set; }
        public decimal? ReprocessQty { get; set; }
        public decimal? ReprocessBaseQty { get; set; }
        public decimal ConversionQty { get; set; }
        public string Lot { get; set; }
        public Guid? LocationID { get; set; }
        public Guid? DMFromWarehouse { get; set; }
        public Guid? WorkerID { get; set; }
        public Guid? ProductStatusID { get; set; }
        public DateTime DamageDate { get; set; }
        public decimal DamageQty { get; set; }
        public decimal DamageBaseQty { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? MFGDate { get; set; }
        public decimal? RejectQty { get; set; }
        public decimal? RejectBaseQty { get; set; }
        public bool DispatchRejectStatus { get; set; }
        public bool DispatchReprocessStatus { get; set; }


        public Guid? ApproveBy { get; set; }
        public bool? IsSentInterfaceChangeLot { get; set; }
        public bool? IsSentInterfaceChangeStatus { get; set; }
        [NotMapped]
        public string ProductCode { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public string LineCode { get; set; }
        [NotMapped]
        public string LocationNo { get; set; }
        [NotMapped]
        public string WarehouseName { get; set; }

        [NotMapped]
        public string ReasonName { get; set; }

        [NotMapped]
        public virtual Reason Reason { get; set; }
        [NotMapped]
        public string UnitName { get; set; }
        [NotMapped]
        public string StatusName { get; set; }
        [NotMapped]
        public bool IsReject { get; set; }
        [NotMapped]
        public bool IsReprocess { get; set; }
    }
}
