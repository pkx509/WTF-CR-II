using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductTemplateUOMService : IRepository<ProductTemplateUom>
    {
        ProductTemplateUom GetById(Guid id);
        List<ProductTemplateUom> GetProductTemplateUom(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductTemplateUomDetail> GetProductTemplateUomDetail(Guid productuomtemplateid);
        void AddProductTemplateUom(ProductTemplateUom entity);
        void ModifyProductTemplateUom(ProductTemplateUom entity);
        void RemoveProductTemplateUom(Guid id);
    }
}
