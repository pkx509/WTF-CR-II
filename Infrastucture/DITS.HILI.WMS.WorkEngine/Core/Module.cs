using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
    public class Module : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ModuleGroupID { get; set; }

        public virtual ModuleGroup ModuleGroup { get; set; }

        public virtual ICollection<WorkFlow> WorkFlowSource { get; set; }
        public virtual ICollection<WorkFlow> WorkFlowDestination { get; set; }
        public Module()
        {
            WorkFlowSource = new List<WorkFlow>();
            WorkFlowDestination = new List<WorkFlow>();
        }
    }
}
