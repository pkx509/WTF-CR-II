using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public interface ICustomerOfProductOwnerService : IRepository<CustomerOfProductOwner>
    {
        List<CustomerOfProductOwner> getByProductOwnerId(Guid id, out int totalRecords, int? pageIndex, int? pageSize);
        List<CustomerOfProductOwner> getByCustomerId(Guid id, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
