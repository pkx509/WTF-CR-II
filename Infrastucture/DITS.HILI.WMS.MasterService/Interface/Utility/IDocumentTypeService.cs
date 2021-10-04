using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.CustomModel;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IDocumentTypeService : IRepository<DocumentType>
    {
        DocumentType Get(Guid id);
        List<DocumentType> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DocumentType> GetByDocTypeEnum(DocumentTypeEnum doctype, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DocumentType> GetByDocTypeEnumWithAll(DocumentTypeEnum doctype, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DocumentType> GetReceiveType(DocumentTypeEnum doctype, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DocumentTypeCustomModel> GetForInternalReceive(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<DocumentType> GetRefDispatchType(Guid documentID, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
