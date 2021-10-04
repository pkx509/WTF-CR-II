using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Companies
{
    public interface IEmployeeService : IRepository<Employee>
    {
        Employee Get(Guid id);
        List<Employee> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
