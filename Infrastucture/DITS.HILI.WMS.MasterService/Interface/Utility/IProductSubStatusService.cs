using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IProductSubStatusService : IRepository<ProductSubStatus>
    {
        ProductSubStatus Get(Guid id);

        List<ProductSubStatus> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
