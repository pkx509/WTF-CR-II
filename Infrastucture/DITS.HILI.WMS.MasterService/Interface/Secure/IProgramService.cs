using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IProgramService : IRepository<Program>
    {
        Program Get(Guid id);
        List<Program> GetAll(ProgramType programType, string langCode, out int totalRecords, int? pageIndex, int? pageSize);
        List<Program> GetAll(Guid appId, Guid userId, string langCodes);
    }
}
