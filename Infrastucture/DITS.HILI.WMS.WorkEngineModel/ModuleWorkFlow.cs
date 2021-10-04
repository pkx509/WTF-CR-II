using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel
{
    public class ModuleWorkFlow
    {
        public Guid ModuleWorkFlowID { get; set; }
        public Guid? DocumentTypeID { get; set; }
        public Guid ModuleID { get; set; }
        public string Method { get; set; }
        public int Sequence { get; set; }

        public virtual Module Module { get; set; }

    }
}
