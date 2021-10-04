using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
{
    public class WorkFlow
    {
        public Guid WorkFlowID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public Guid? PackagePrevID { get; set; }
        public Guid? PackageNextID { get; set; }
        public int Sequence { get; set; }
        public bool Start { get; set; }

        public virtual Package PackagePrev { get; set; }
        public virtual Package PackageNext { get; set; }

        public WorkFlow()
        {
        }
    }
}
