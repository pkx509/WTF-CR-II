using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Com
{
    public interface ICompanyService : IRepository<Company>
    {
        Company Get(Guid id);
        List<Company> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }

}
