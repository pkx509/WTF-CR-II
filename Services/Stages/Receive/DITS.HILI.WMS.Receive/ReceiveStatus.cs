using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.ReceiveModel
{
    public class ReceiveStatus
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public ReceiveStatusGroup GroupStatus { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public virtual ICollection<Receive> ReceiveCollection { get; set; }
        [NotMapped]
        public virtual ICollection<ReceiveDetail> ReceiveDetailCollection { get; set; }
        [NotMapped]
        public virtual ICollection<Receiving> ReceivingCollection { get; set; }

        public ReceiveStatus()
        {
            ReceiveCollection = new List<Receive>();
            ReceiveDetailCollection = new List<ReceiveDetail>();
            ReceivingCollection = new List<Receiving>();
        }
    }

    public class ReceiveEnumerable
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
