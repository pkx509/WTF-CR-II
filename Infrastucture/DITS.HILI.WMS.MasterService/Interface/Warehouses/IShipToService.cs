using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Warehouses;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Warehouses
{
    public interface IShipToService : IRepository<ShippingTo>
    {
        ShippingTo GetById(Guid id);
        List<ShippingTo> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<ShipToModel> GetShipToRule(string keyword, bool Active, out int totalRecords, int? pageIndex, int? pageSize);
        void Add(ShippingTo entity);
        void Modify(ShippingTo entity);
        void Remove(Guid id);

    }
}
