using DITS.HILI.WMS.MasterModel;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine
{
    public class EngineContext
    {
        public bool ActionTransmit(IWorkEngine _iwork, List<DataTransfer> data)
        {
            bool ok = _iwork.OnReceiveData(data);
            return ok;
        }
    }
}
