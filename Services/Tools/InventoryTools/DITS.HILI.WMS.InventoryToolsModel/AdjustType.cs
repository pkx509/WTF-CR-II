using DITS.HILI.WMS.MasterModel;

namespace DITS.HILI.WMS.InventoryToolsModel
{
    public class AdjustType : BaseEntity
    {
        public System.Guid AdjustTypeID { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
