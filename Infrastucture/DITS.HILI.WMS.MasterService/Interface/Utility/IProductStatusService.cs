using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IProductStatusService : IRepository<ProductStatus>
    {
        ProductStatus Get(Guid id);
        List<ProductStatus> GetByDocuemtnType(Guid documentTypeId);
        List<ProductStatus> Get(string keyword, Guid? documentTypeID, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
