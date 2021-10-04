using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class Truck : BaseEntity
    {
        public Guid TruckID { get; set; }
        public Guid? TruckTypeID { get; set; }
        public string TruckNo { get; set; }
        public int? ProvinceID { get; set; }

        public virtual TruckType TruckType { get; set; }
    }
}
