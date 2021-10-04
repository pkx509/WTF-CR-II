using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
{
    [NotMapped]
    public class WorkFlowRuntime
    {
        public static List<WorkFlow> WorkFlowCollection { get; set; }

    }
}
