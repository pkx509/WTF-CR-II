using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductBrandService : IRepository<ProductBrand>
    {
        ProductBrand Get(Guid id);
        List<ProductBrand> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
