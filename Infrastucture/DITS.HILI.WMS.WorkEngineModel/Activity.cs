using DITS.HILI.WMS.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.WorkEngineModel
{
    public class Activity
    {
        public Guid ActivityID { get; set; }
        public string ReferenceID { get; set; }
        public Guid Source { get; set; }
        public Guid Destination { get; set; }
        public int Sequence { get; set; }
        public bool IsComplete { get; set; }
        public DateTime ActivityDate { get; set; }
         
    }
}
