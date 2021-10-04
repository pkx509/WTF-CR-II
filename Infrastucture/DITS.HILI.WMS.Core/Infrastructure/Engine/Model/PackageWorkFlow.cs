using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
{
    public class PackageWorkFlow
    {
        public Guid PackageWorkFlowID { get; set; }
        public Guid? DocumentTypeID { get; set; }
        public Guid PackageID { get; set; }
        public string Method { get; set; }
        public int Sequence { get; set; }

        public virtual Package Package { get; set; }

    }
}
