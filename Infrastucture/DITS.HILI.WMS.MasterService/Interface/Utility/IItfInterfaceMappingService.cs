using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IItfInterfaceMappingService : IRepository<ItfInterfaceMapping>
    {
        List<ItfInterfaceMapping> GetByDocument(Guid id);
    }
}
