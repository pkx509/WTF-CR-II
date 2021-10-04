using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.Domain;
using DITS.HILI.WMS.MasterModel.Products;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IProductService : IRepository<Product>
    {
        Product Get(Guid? productOwnerId, Guid id);
        Product GetAllByID(Guid? productOwnerId, Guid id);
        List<Product> Get(Guid? productOwnerId, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Product> GetByStockCode(Guid? productOwnerId, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductModel> GetAll(Guid? productOwnerId, Guid? productId, Guid? brandId, Guid? shapeId, Guid? groupLV3Id, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<ProductCustomModel> GetProductSelectAll(string productCode, string productName, out int totalRecords, int? pageIndex, int? pageSize);
        ApiResponseMessage AddProduct(Product entity);
        ApiResponseMessage ModifyProduct(Product entity);
        void RemoveProduct(Guid id);

        List<ProductCustomModel> GetProductForInternalRec(string PONo, string ProductCode, string ProductName
                                                            , bool IsCreditNote, bool IsNormal, bool ToReprocess
                                                            , bool FromReprocess, bool IsItemChange
                                                            , bool IsWithoutGoods, Guid? ReferenceDispatchTypeID
                                                            , out int totalRecords, int? pageIndex, int? pageSize);
    }
}
