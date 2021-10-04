using System;

namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class ConditionConfigModel
    {
        public Guid ConfigID { get; set; }
        public string ModuleName { get; set; }
        public string ConfigName { get; set; }
        public string ConfigVariable { get; set; }
        public bool? isComboBox { get; set; }
        public string ConfigScript { get; set; }

    }

    public class DataKeyValue
    {
        public string Key { get; set; }
        public Guid Value { get; set; }
    }
    public class DataKeyValueString
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
