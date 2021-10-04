using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayReason
    {
        public Guid PutAwayReasonID { get; set; }
        public string Description { get; set; }
         
        public virtual ICollection<PutAwayConfirm> PutAwayConfirmCollection { get; set; }

        public PutAwayReason()
        {
            PutAwayConfirmCollection = new List<PutAwayConfirm>();
        }
    }
}
