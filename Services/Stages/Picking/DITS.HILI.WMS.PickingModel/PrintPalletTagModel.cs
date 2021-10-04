using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.PickingModel
{
    public class PrintPalletTagModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Line { get; set; }
        public string OrderNo { get; set; }
        public string ProductionDate { get; set; }
        public string ProductTime { get; set; }
        public string Quantity { get; set; }
        public string UnitName { get; set; }
        public string LotNo { get; set; }
        public string PalletTag { get; set; }
        public string DockLoad { get; set; }
    }
}