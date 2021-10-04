using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayMethodConfig : BaseEntity
    {
        public Guid MethodID { get; set; }
        public string Name { get; set; } 
        public int Sequence { get; set; }
        public string Method { get; set; }
    }
}
