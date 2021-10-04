using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IRoleService : IRepository<Roles>
    {
        Roles Get(Guid id);
        void SavePermission(List<PermissionInRole> entity);
        void Delete(Guid id);
        List<Roles> GetAll(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<PermissionInRole> GetPermission(Guid roleID);
    }
}
