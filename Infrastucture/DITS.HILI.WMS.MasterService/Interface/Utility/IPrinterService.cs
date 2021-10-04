using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.MasterModel.Utility;
using DITS.WMS.Data.CustomModel;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IPrinterService : IRepository<Printer>
    {
        Printer GetById(Guid id);
        List<PrinterModel> Get(Guid? lineID, string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<BasePrinter> GetPrinterMachine(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<Line> GetPrinterLocation(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        void AddPrinter(Printer entity);
        void ModifyPrinter(Printer entity);
        void RemovePrinter(Guid id);
    }
}
