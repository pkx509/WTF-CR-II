using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Rule;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Rule
{
    public interface IRuleService : IRepository<SpecialPutawayRule>
    {
        SpecialPutawayRule Get(Guid id);
        List<SpecialPutawayRule> Get(Guid? putAwayRuleID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);

        List<SpecialBookingRule> GetBookingRule(string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
        void Add(SpecialBookingRule entity);
        void Modify(SpecialBookingRule entity);
        void Remove(Guid id);

    }
}
