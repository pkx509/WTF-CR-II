using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel
{
    public class Module
    {
        public Guid ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ModuleGroupID { get; set; }

        public virtual ModuleGroup ModuleGroup { get; set; }

        public virtual ICollection<WorkFlow> WorkFlowSourceCollection { get; set; }
        public virtual ICollection<WorkFlow> WorkFlowDestinationCollection { get; set; }
        public virtual ICollection<ModuleWorkFlow> ModuleWorkFlowCollection { get; set; }
        public Module()
        {
            WorkFlowSourceCollection = new List<WorkFlow>();
            WorkFlowDestinationCollection = new List<WorkFlow>();
            ModuleWorkFlowCollection = new List<ModuleWorkFlow>();
        }
    }
}
