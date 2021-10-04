using DITS.HILI.WMS.MasterModel;
using System;

namespace DITS.HILI.WMS.PickingModel
{
    public class PickingMethod : BaseEntity
    {
        public Guid MethodID { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
    }
}
