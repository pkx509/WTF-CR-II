using DITS.HILI.WMS.MasterModel.Warehouses;
using System;
using System.Collections.Generic;

namespace DITS.HILI.WMS.MasterModel.Utility
{
    public class PalletType : BaseEntity
    {
        public Guid PalletTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double MaxWeight { get; set; }
        public bool IsDefault { get; set; }

        public virtual ICollection<Location> LocationCollection { get; set; }

        public PalletType()
        {
            LocationCollection = new List<Location>();
        }
    }
}
