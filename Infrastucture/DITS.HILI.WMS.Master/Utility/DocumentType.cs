using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class DocumentType : BaseEntity
    {
        public Guid DocumentTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentTypeEnum DocType { get; set; }
        public bool? IsDefault { get; set; }


        [NotMapped]
        public string FullName { get; set; }

        public virtual ICollection<ProductStatusMapDocument> ProductStatusMapDocumentCollection { get; set; }
        public DocumentType()
        {
            ProductStatusMapDocumentCollection = new List<ProductStatusMapDocument>();
        }

    }
}
