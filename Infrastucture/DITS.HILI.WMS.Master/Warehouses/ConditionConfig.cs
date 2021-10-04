namespace DITS.HILI.WMS.MasterModel.Warehouses
{
    public class ConditionConfig
    {
        public System.Guid ConfigId { get; set; } // ConfigID (Primary key)
        public string ModuleName { get; set; } // ModuleName (length: 50)
        public string ConfigName { get; set; } // ConfigName (length: 50)
        public int PrioritySeq { get; set; } // PrioritySeq
        public bool IsOrder { get; set; } // IsOrder
        public System.Guid? ConfigVariableId { get; set; }
        public string ConfigVariable { get; set; } // ConfigVariable (length: 50)
        public bool? IsComboBox { get; set; } // IsComboBox
        public string ConfigScript { get; set; } // ConfigScript (length: 250)
    }
}
