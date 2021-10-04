using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DailyPlanService
{
    public interface IProductionLineService : IRepository<Line>
    {
        Line Get(Guid id);
        List<Line> GetAll(string keyword, bool Active, LineTypeEnum lineType, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
