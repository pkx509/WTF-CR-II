using System;

namespace DITS.WMS.Data.CustomModel
{
    public class BasePrinter
    {
        public string PrinterName { get; set; }
        public bool OnLine { get; set; }
        public bool IsDefault { get; set; }
    }

    public class PrinterModel
    {
        public Guid PrinterId { get; set; } // PrinterID (Primary key)
        public string PrinterName { get; set; } // PrinterName (length: 250)
        public string PrinterLocation { get; set; } // PrinterLocation (length: 50)
        public string Description { get; set; } // Description
        public bool? IsDefault { get; set; } // IsDefault
        public string Location_Name { get; set; }
        public string Location_Loading { get; set; }
        public int OutCount { get; set; }
        public bool IsDriverInstall
        {
            get;
            set;
        }
        public bool IsOnLine { get; set; }
        public string DriverInstall => (IsDriverInstall ? "[ Installed ]" : "[ Not Installation ]");
    }
}
