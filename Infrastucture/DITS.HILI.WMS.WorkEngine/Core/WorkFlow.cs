using DITS.HILI.WMS.Master.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
    public class WorkFlow
    {
        public Guid WorkFlowID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public Guid InstanceSourceID { get; set; }
        public int Sequece { get; set; }
        public int InSequence { get; set; }
        public int OutSequence { get; set; }
        public Guid? InstanceDestinationID { get; set; }
        public string Description { get; set; }

        public virtual Module ModuleSource { get; set; }
        public virtual Module ModuleDestination { get; set; }
        public virtual DocumentType DocumentType { get; set; }

        public WorkFlow()
        {
        }
    }
}
