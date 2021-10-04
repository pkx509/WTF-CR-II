using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    /// <summary>
    /// รอตรวจสอบว่าจะเอาไปใช้ทำอะไร 19/04/2017
    /// </summary>
    public class ZoneConfig
    {
        public Guid ConfigID { get; set; }
        public string ConfigName { get; set; }
        public Guid ZoneID { get; set; }
        public int PrioritySeq { get; set; }
        public bool IsOrder { get; set; }
        public string ConfigVariable { get; set; }
        public bool IsCombobox { get; set; }
        public string ConfigScript { get; set; }
    }
}
