using DITS.HILI.WMS.MasterModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{

    public class TRMTransferMarketingProductDetail : BaseEntity
    {
        [Key]
        public Guid? TRM_Product_Detail_ID { get; set; }

        public Guid? TRM_Product_ID { get; set; }

        public Guid? ProductID { get; set; }

        public decimal? PalletQty { get; set; }

        public Guid? PalletUnitID { get; set; }

        public Guid? PalletBaseUnitID { get; set; }

        public decimal? PalletBaseQty { get; set; }

        public decimal? PickQty { get; set; }
        public decimal? OrderPickQty { get; set; }

        public decimal? ConfirmPickQty { get; set; }

        public string LotNo { get; set; }

        [StringLength(40)]
        public string PalletCode { get; set; }

        [StringLength(40)]
        public string NewPalletCode { get; set; }

        public int? PickStatus { get; set; }

        public Guid? LocationID { get; set; }


        public virtual TRMTransferMarketingProduct TRMTransferMarketingProduct { get; set; }


        [NotMapped]
        public Guid? TRM_ID { get; set; }
        [NotMapped]
        public string TRM_Code { get; set; }
        [NotMapped]
        public string TransferDetailStatusName { get; set; }
        [NotMapped]
        public string ProductCode { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string ProductUnitName { get; set; }
        [NotMapped]
        public Guid ProductStatusID { get; set; }
        [NotMapped]
        public Guid ProductOwnerID { get; set; }
        [NotMapped]
        public decimal ConversionQty { get; set; }
        [NotMapped]
        public Guid PackingID { get; set; }
        [NotMapped]
        public Guid ProductUnitID { get; set; }
        [NotMapped]
        public Guid BaseUnitId { get; set; }

        [NotMapped]
        public string Location { get; set; }
        [NotMapped]
        public decimal? SumPickConfQty { get; set; }
        [NotMapped]
        public decimal? TransferQty { get; set; }
    }
}
