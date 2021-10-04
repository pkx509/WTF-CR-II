using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.InventoryToolsModel
{

    public class TRMTransferMarketing : BaseEntity
    {
        public TRMTransferMarketing()
        {
            TRMTransferMarketingProduct = new HashSet<TRMTransferMarketingProduct>();
        }

        [Key]
        public Guid TRM_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string TRM_CODE { get; set; }

        public DateTime TransferDate { get; set; }

        public DateTime? ApproveDate { get; set; }

        public int? TransferStatus { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public Guid? UserCreated { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? UserModified { get; set; }

        public DateTime? DateModified { get; set; }

        public bool? IsActive { get; set; }
        public virtual ICollection<TRMTransferMarketingProduct> TRMTransferMarketingProduct { get; set; }

        [NotMapped]
        public string TransferStatusName { get; set; }
        [NotMapped]
        public bool? IsApprove { get; set; }


    }
}
