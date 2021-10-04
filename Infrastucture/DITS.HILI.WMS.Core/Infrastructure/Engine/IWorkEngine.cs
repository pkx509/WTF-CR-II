using DITS.HILI.WMS.MasterModel;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine
{
    public interface IWorkEngine
    {
        bool OnReceiveData(List<DataTransfer> data);

    }
}
