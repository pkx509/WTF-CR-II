using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IReasonService : IRepository<Reason>
    {
        List<Reason> GetReasons(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
