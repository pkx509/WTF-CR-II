using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IWarehouseService : IRepository<Warehouse>
    {
        Warehouse Get(Guid id);
        List<Warehouse> Get(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Warehouse> GetMk(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<WarehouseModel> GetAll(Guid? warehouseTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        WarehouseType GetWarehouseTypByID(Guid warehouseTypeId);
        List<WarehouseType> GetWarehouseType(Guid? warehouseTypeId, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<WarehouseModel> GetWarehouseTypeAll(Guid? warehouseTypeId, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);

        void AddWarehouseType(WarehouseType entity);
        void WarehouseTypeModify(WarehouseType entity);
        void WarehouseTypeRemove(Guid id);


    }
}
