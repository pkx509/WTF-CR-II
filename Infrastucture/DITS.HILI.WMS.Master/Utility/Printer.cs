using System;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class Printer : BaseEntity
    {
        public Guid PrinterId { get; set; } // PrinterID (Primary key)
        public string PrinterName { get; set; } // PrinterName (length: 250)
        /// <summary>
        /// PrinterLocationID = LineID
        /// </summary>
        public Guid? PrinterLocationId { get; set; }
        public string PrinterLocation { get; set; } // PrinterLocation (length: 50)
        public string Description { get; set; } // Description
        public bool? IsDefault { get; set; } // IsDefault
    }
}
