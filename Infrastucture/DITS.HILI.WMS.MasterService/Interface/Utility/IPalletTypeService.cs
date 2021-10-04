using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.MasterModel.Utility;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterService.Utility
{
    public interface IPalletTypeService : IRepository<PalletType>
    {
        PalletType Get(Guid id);
        List<PalletType> Get(string keyword, out int totalRecords, int? pageIndex, int? pageSize);
    }
}
