using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{

    public class TRMTransferMarketingProduct : BaseEntity
    {
        public TRMTransferMarketingProduct()
        {
            TRMTransferMarketingProductDetail = new HashSet<TRMTransferMarketingProductDetail>();
        }

        [Key]
        public Guid? TRM_Product_ID { get; set; }

        public Guid? TRM_ID { get; set; }

        public Guid? Product_ID { get; set; }

        public decimal? TransferQty { get; set; }

        public Guid? TransferUnitID { get; set; }
        public Guid? ProductStatusID { get; set; }

        public decimal? TransferBaseQty { get; set; }

        public Guid? TransferBaseUnitID { get; set; }

        public decimal? PickQty { get; set; }

        public decimal? ConfirmQty { get; set; }

        public int? PickStatus { get; set; }

        public bool? IsActive { get; set; }

        public Guid? UserCreated { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? UserModified { get; set; }

        public DateTime? DateModified { get; set; }

        public virtual TRMTransferMarketing TRMTransferMarketing { get; set; }
        public virtual ICollection<TRMTransferMarketingProductDetail> TRMTransferMarketingProductDetail { get; set; }


        [NotMapped]
        public string TransferStatusName { get; set; }
        [NotMapped]
        public bool IsApprove { get; set; }
        [NotMapped]
        public string ProductCode { get; set; }
        [NotMapped]
        public string TRM_CODE { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
        [NotMapped]
        public string ProductUnitName { get; set; }
        [NotMapped]
        public DateTime TransferDate { get; set; }
        [NotMapped]
        public DateTime? ApproveDate { get; set; }
        [NotMapped]
        public int? TransferStatus { get; set; }
        [NotMapped]
        public decimal? TotalPickQty { get; set; }
        [NotMapped]
        public decimal? TotalConfirmQty { get; set; }
        [NotMapped]
        public string ProductStatusName { get; set; }
        [NotMapped]
        public System.Guid? ProductSubStatusId { get; set; }
        [NotMapped]
        public string ProductSubStatusName { get; set; }
        [NotMapped]
        public System.Guid? ProductOwnerId { get; set; }
        [NotMapped]
        public string ProductOwnerName { get; set; }
        [NotMapped]
        public decimal? ProductWidth { get; set; }
        [NotMapped]
        public decimal? ProductLength { get; set; }
        [NotMapped]
        public decimal? ProductHeight { get; set; }
        [NotMapped]
        public decimal? ProductWeight { get; set; }
        [NotMapped]
        public decimal? PackageWeight { get; set; }
        [NotMapped]
        public System.Guid? PriceUnitId { get; set; }
        [NotMapped]
        public string PriceUnitName { get; set; }
        [NotMapped]
        public decimal? Price { get; set; }
        [NotMapped]
        public decimal RemainQTY { get; set; }
        [NotMapped]
        public decimal OverQTY { get; set; }
    }
}
