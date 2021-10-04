using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductShapeService : IRepository<ProductShape>
    {
        ProductShape Get(Guid id);
        List<ProductShape> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
