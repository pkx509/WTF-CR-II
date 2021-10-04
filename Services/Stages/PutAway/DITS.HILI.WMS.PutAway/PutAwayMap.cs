using DITS.HILI.WMS.MasterModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DITS.HILI.WMS.PutAwayModel
{
    public class PutAwayMap : BaseEntity
    {
        public Guid PutAwayID { get; set; }
        public Guid PutAwayItemID { get; set; }
        [NotMapped]
        public virtual PutAway PutAway { get; set; }
        [NotMapped]
        public virtual PutAwayItem PutAwayItem { get; set; }

    }
}
