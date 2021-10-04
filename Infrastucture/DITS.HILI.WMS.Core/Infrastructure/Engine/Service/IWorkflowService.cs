using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.PackagesModel;
using System.Collections.Generic;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Service
{
    public interface IWorkflowService : IRepository<WorkFlow>
    {
        void Load();
        List<WorkFlow> Get();
    }
}
