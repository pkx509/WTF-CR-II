using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.ReceiveModel;
using System;

namespace DITS.HILI.WMS.ReceiveService
{
    public interface IReceiveDetailService : IRepository<ReceiveDetail>
    {
        bool Cancel(Guid id);
    }
}
