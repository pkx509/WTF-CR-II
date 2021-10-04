using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DailyPlanService
{
    public interface IDailyPlanService : IRepository<ProductionPlan>
    {
        List<ValidationImportFileResult> ValidateImportDailyPlan(List<ProductionPlanCustomModel> item);

        bool SendToReceive(List<ProductionPlanCustomModel> item);

        bool ImportDailyPlan(List<ProductionPlanCustomModel> item);

        bool DeletePlan(List<Guid> items);

        bool SaveData(ProductionPlanCustomModel data);

        ProductionPlanCustomModel GetByID(Guid id);

        List<Location> GetLocationByLine(Guid lineID, LocationTypeEnum? locationType, string keyword, out int totalRecords, int? pageIndex, int? pageSize);

        List<ProductionPlanCustomModel> GetAll(DateTime? sdte, DateTime? edte, Guid? lineId, LineTypeEnum lineType, bool isReceive, string keyword, out int totalRecords, int? pageIndex, int? pageSize);

        void SedToReceive(List<Guid> ids);
    }
}
