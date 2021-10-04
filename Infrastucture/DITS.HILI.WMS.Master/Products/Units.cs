using System;

namespace DITS.HILI.WMS.MasterModel.Products
{
    public class Units : BaseEntity
    {
        public Guid UnitID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }


        public Units()
        {
        }

    }
}
