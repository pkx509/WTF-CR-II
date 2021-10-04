using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayModel
{
    [NotMapped]
    public class JobPutAway
    {
        public string JobPutAwayNo { get; set; }
        public DateTime PutAwayDate { get; set; }
        public List<Guid> EmployeeID { get; set; }

    }
}
