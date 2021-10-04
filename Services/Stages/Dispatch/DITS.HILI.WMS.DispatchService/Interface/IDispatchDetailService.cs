using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.DispatchModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using System.Collections.Generic;

namespace DITS.HILI.WMS.DispatchService
{
    public interface IDispatchDetailService : IRepository<DispatchDetail>
    {
        bool AddList(List<DispatchDetailCustom> dispatchDetails);
    }
}
