using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Products;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Products
{
    public interface IUnitService : IRepository<Units>
    {
        Units Get(Guid? id);
        List<Units> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Units> GetUnit(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        List<Units> GetUnitCombo(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void AddUnit(Units entity);
        void ModifyUnit(Units entity);
        void RemoveUnit(Guid id);
    }
}
