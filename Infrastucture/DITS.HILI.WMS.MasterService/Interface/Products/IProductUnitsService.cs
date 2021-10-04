using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductUnitsService : IRepository<ProductUnit>
    {
        ProductUnit Get(Guid id);
        List<ProductUnit> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductUnit> GetProductUnitAll(Guid? productUnitId, Guid? productId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void AddUnit(ProductUnit entity);
        void ModifyUnit(ProductUnit entity);
        void RemoveUnit(Guid id);
    }
}
