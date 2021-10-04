using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IPermissionService : IRepository<Permission>
    {
        Permission Get(Guid id);
        List<Permission> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
