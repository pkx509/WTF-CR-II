using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Secure;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Secure
{
    public interface IMonthEndService : IRepository<Monthend>
    {
        List<Monthend> GetAll(Guid monthendId);
        List<Monthend> GetAll(out int totalRecord);
        bool CanUsed(DateTime dateRef);
       // Monthend GetLastMonthend(DateTime dateRef);
        Monthend CreateOrUpdate(Monthend entity);
        bool Remove(Monthend entity);
        List<Monthend> GetAll(out int totalRecord, int? pageIndex, int? pageSize);
        bool CheckCutoffDate(DateTime dateref);
        bool Active(Monthend entity);

        bool InActive(Monthend entity);
    }
}
