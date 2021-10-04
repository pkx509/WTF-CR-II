using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Companies
{

    public interface ISiteService : IRepository<Site>
    {
        Site Get(Guid id);
        List<Site> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
        List<SiteModel> GetAll(Guid? id, string keyword, bool IsActive, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
