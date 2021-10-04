using DITS.HILI.WMS.MasterModel;
using DITS.HILI.WMS.MasterModel.Companies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayJobAssign : BaseEntity
    {
        public Guid PutAwayJobID { get; set; }
        public Guid EmployeeID { get; set; }


        public PutAway PutAway { get; set; }

        [NotMapped]
        public Employee Employee { get; set; }
    }
}
