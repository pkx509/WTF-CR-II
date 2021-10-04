using System;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class ItfInterfaceMapping : BaseEntity
    {
        public Guid InterfaceTypeId { get; set; } // InterfaceTypeID (Primary key)
        public Guid DocumentId { get; set; } // DocumentID (Primary key)
        public bool? IsRegistTruck { get; set; } // IsRegistTruck
        public bool? IsAssign { get; set; } // IsAssign
        public bool? IsMarketing { get; set; } // IsMarketing
        public bool? IsCreditNote { get; set; }
        public bool? IsNormal { get; set; }
        public bool? ToReprocess { get; set; } // ToReprocess
        public bool? FromReprocess { get; set; } // FromReprocess
        public bool? IsItemChange { get; set; }
        public bool? IsWithoutGoods { get; set; }
        public bool? IsQADamage { get; set; }
        public bool? IsQAReprocessFromDamage { get; set; }
        public bool? IsQAReprocessFromHold { get; set; }
        public bool IsActive { get; set; } // IsActive
        public string Remark { get; set; } // Remark (length: 250)
        public Guid? ReferenceDocumentID { get; set; }
        public Guid UserCreated { get; set; } // UserCreated
        public DateTime DateCreated { get; set; } // DateCreated
        public Guid UserModified { get; set; } // UserModified
        public DateTime DateModified { get; set; } // DateModified
        public bool? IsDisplay { get; set; }

        // Foreign keys
        public virtual ItfTransactionType ItfTransactionType { get; set; } // FK_InterfaceTypeID


    }
}
