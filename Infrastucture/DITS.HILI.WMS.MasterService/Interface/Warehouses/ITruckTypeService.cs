using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface ITruckTypeService : IRepository<TruckType>
    {
        TruckType Get(Guid id);
        TruckNoModel GetTruckNobyid(Guid id);
        List<TruckType> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize, bool IsActive = true);
        List<TruckNoModel> GetTruckNolist(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);

        bool AddTruckNo(Truck entity);
        bool ModifyTruckNo(Truck entity);
        bool RemoveTruckNo(Guid id);
    }
}
