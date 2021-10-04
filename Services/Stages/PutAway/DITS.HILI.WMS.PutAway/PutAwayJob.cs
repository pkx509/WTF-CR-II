using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.PutAwayModel
{

    public class PutAwayJob
    {
        public Guid GroupID { get; set; }
        public Guid Warehouse { get; set; }
        public Guid Zone { get; set; }
        public Guid Product { get; set; }
        public List<PutAwayItem> Item { get; set; }
    }
}
