using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductGroupLevelService : IRepository<ProductGroupLevel3>
    {
        ProductGroupLevel3 Get(Guid id);
        List<ProductGroupLevel3> Get(Guid? groupLV2Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductGroupLevel3> GetAll(Guid? groupLV2Id, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductGroupLevel2> GetProductGroupLevel2(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
