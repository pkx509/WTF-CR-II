using DITS.HILI.WMS.MasterModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class ReclassifiedDetail : BaseEntity
    {
        public System.Guid ReclassifiedDetailID { get; set; }
        public Guid ReclassifiedID { get; set; }
        public string PalletCode { get; set; }
        public string FromPalletCode { get; set; }
        public decimal ReclassifiedQty { get; set; }
        public Guid ReclassifiedUnitID { get; set; }
        public decimal ReclassifiedBaseQty { get; set; }
        public Guid ReclassifiedBaseUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        //public ReclassifiedDetailStatus ReclassifiedDetailStatus { get; set; }


        public string ReprocessPalletCode { get; set; }
        public string RejectPalletCode { get; set; }
        public string ReprocessLot { get; set; }
        public string RejectLot { get; set; }

        public Guid? ReFromWarehouse { get; set; }
        public Guid? ReFromLoction { get; set; }
        // [NotMapped]
        //public decimal RejectQty { get; set; } 
        public decimal TotalRejectQty { get; set; }
        public decimal TotalRejectBaseQty { get; set; }

        //[NotMapped]
        // public decimal ReprocessQty { get; set; } 
        public decimal TotalReprocessQty { get; set; }
        public decimal TotalReprocessBaseQty { get; set; }
        [NotMapped]
        public virtual Changestatus Changestatus { get; set; }

        public virtual Reclassified Reclassified { get; set; }
        [NotMapped]
        public bool IsReject { get; set; }
        [NotMapped]
        public bool IsReprocess { get; set; }
        public bool? IsSentInterfaceChangeLot { get; set; }
        public bool? IsSentInterfaceChangeStatus { get; set; }
    }
}
