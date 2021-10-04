using DITS.HILI.WMS.Core.Data;
using DITS.HILI.WMS.Core.PackagesModel;
using System.Collections.Generic;
using System.Linq;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Service
{
    public class WorkflowService : Repository<WorkFlow>, IWorkflowService
    {
        public WorkflowService(IUnitOfWork context)
            : base(context)
        {
        }

        public void Load()
        {
            List<WorkFlow> flow = Query().Get().ToList();
            WorkFlowRuntime w = new WorkFlowRuntime();
            WorkFlowRuntime.WorkFlowCollection.AddRange(flow);
        }

        public override WorkFlow FindByID(object id)
        {
            return base.FindByID(id);
        }

        public List<WorkFlow> Get()
        {
            return Query().Get().ToList();
        }
    }
}
