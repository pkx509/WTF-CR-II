using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core
{
    public class Activity
    {
        public Guid ActivityID { get; set; }
        public string ReferenceID { get; set; }
        public Guid InstanceSourceID { get; set; }
        public Guid InstanceDestinationID { get; set; }
        public int Sequence { get; set; }
        public bool IsComplete { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
