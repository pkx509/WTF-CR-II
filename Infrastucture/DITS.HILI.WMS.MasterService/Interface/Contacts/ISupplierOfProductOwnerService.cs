using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Contacts;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Contacts
{
    public interface ISupplierOfProductOwnerService : IRepository<SupplierOfProductOwner>
    {
        List<SupplierOfProductOwner> getByProductOwnerId(Guid id, out int totalRecords, int? pageIndex, int? pageSize);
        List<SupplierOfProductOwner> getBySupplierId(Guid id, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
