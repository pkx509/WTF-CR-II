using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public interface IProductOwnerService : IRepository<ProductOwner>
    {
        ProductOwner Get(Guid id);
        List<ProductOwner> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
