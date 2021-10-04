using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
    public class InternalWorkFlow : BaseEntity
    {
        public Guid DocumentTypeID { get; set; }
        public Guid ModuleID { get; set; }
        public string Method { get; set; }
        public int Sequence { get; set; }
    }
}
