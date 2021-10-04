using DITS.HILI.WMS.MasterModel.Utility;
using System;

namespace DITS.HILI.WMS.MasterModel.CustomModel
{
    public class DocumentTypeCustomModel
    {
        public Guid DocumentTypeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentTypeEnum DocType { get; set; }
        public Guid? RefDocumentID { get; set; }
        public bool? IsCreditNote { get; set; }
        public bool? IsNormal { get; set; }
        public bool? ToReprocess { get; set; }
        public bool? FromReprocess { get; set; }
        public bool? IsItemChange { get; set; }
        public bool? IsWithoutGoods { get; set; }

        public virtual ProductStatus ProductStatus { get; set; }
    }
}
