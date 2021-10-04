using System;

namespace DITS.HILI.WMS.Core.PackagesModel
{

    public class WorkFlow
    {
        public Guid WorkFlowID { get; set; }
        public Guid DocumentTypeID { get; set; }
        public Guid? PackagePrevID { get; set; }
        public Guid? PackageNextID { get; set; }
        public int Sequence { get; set; }
        public bool Start { get; set; }
        public bool IsStockReserve { get; set; }

        public virtual Package PackagePrev { get; set; }
        public virtual Package PackageNext { get; set; }

        public WorkFlow()
        {
        }
    }
}
