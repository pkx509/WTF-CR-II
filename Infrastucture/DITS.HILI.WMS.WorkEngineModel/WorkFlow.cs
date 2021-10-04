using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel
{
    public class WorkFlow
    {
        public Guid WorkFlowID { get; set; }
        public Guid InstanceID { get; set; }
        public Guid DocumentTypeID { get; set; } 
        public Guid? Source { get; set; }
        public Guid? Destination { get; set; }
        public int Sequence { get; set; }
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
        public string Description { get; set; }

        public virtual Module ModuleSource { get; set; }
        public virtual Module ModuleDestination { get; set; }

        public WorkFlow()
        {
        }
    }
}
