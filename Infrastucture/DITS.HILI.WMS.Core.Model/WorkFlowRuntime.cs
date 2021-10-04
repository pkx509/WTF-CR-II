using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.Core.PackagesModel
{
    [NotMapped]
    public class WorkFlowRuntime
    {
        public static List<WorkFlow> WorkFlowCollection { get; set; }

        public WorkFlowRuntime()
        {
            WorkFlowCollection = new List<WorkFlow>();
        }
    }
}
