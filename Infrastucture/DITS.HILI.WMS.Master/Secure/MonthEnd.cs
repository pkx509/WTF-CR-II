using System;

namespace DITS.HILI.WMS.MasterModel.Secure
{
    public class Monthend : BaseEntity
    {
        public Guid ID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CutOffDate { get; set; }
        public Monthend()
        {

        }
    }
}
