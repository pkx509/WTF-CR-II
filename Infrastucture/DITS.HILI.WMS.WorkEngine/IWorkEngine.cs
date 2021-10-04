using DITS.HILI.WMS.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngine
{
    public interface IWorkEngine
    {
        bool OnReceive(List<DataTransfer> data);
    }
}
