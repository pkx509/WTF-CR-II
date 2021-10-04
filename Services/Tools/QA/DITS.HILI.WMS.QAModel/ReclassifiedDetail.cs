using DITS.HILI.WMS.InventoryToolsModel;
using DITS.HILI.WMS.MasterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.QAModel
{
    public class ReclassifiedDetail : BaseEntity
    {
        public Guid ReclassifiedDetailID { get; set; }
        public Guid ReclassifiedID { get; set; }
        public string PalletCode { get; set; }
        public decimal ReclassifiedQty { get; set; }
        public Guid ReclassifiedUnitID { get; set; }
        public decimal ReclassifiedBaseQty { get; set; }
        public Guid ReclassifiedBaseUnitID { get; set; }
        public decimal ConversionQty { get; set; }
        public int ReclassifiedDetailStatus { get; set; }
        public Guid? DamageID { get; set; }


        [NotMapped]
        public virtual Changestatus Changestatus { get; set; }

        
        public virtual Reclassified Reclassified { get; set; }
    }
}
