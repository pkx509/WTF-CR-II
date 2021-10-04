using System;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class ItemReclassified
    {
        public Guid? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Lot { get; set; }
        public string PalletCode { get; set; }
        public decimal PalletQty { get; set; }
        public decimal ReclassifiedQty { get; set; }
        public Guid ReclassifiedUnitID { get; set; }

        public TimeSpan? MFGTimeFrom { get; set; }
        public TimeSpan? MFGTimeEnd { get; set; }
        public Guid? LineID { get; set; }
        public string LineCode { get; set; }
        public DateTime? MFGDate { get; set; }
        public DateTime? EXPDate { get; set; }
        public Guid? ProductStatusID { get; set; }
        public Guid? FromProductStatusID { get; set; }
        public string Description { get; set; }
        public decimal ReclassifiedBaseQty { get; set; }
        public Guid ReclassifiedBaseUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        public string Location { get; set; }
        public string UnitName { get; set; }
        public DateTime? ApproveDate { get; set; }
        public Guid ReclassifiedDetailID { get; set; }
        public Guid ReclassifiedID { get; set; }
        public string ReclassifiedCode { get; set; }


        public bool IsReject { get; set; }
        public bool IsReprocess { get; set; }
        public decimal RejectQty { get; set; }
        public decimal ReprocessQty { get; set; }
        public decimal TotalRejectQty { get; set; }
        public decimal TotalReprocessQty { get; set; }
    }
}
