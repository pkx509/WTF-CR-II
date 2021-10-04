using DITS.HILI.WMS.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngine
{
    public class EngineContext
    {
        public bool ActionTransmit(IWorkEngine _iwork, List<DataTransfer> data)
        {
            var ok = _iwork.OnReceive(data);
            return ok;
        }
    }
}
