namespace DITS.WMS.Data.CustomModel
{
    public class ShipToModel
    {
        public System.Guid ShipToId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool? IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string BusinessGroup { get; set; }
        public System.Guid RuleId { get; set; }
        public string RuleName { get; set; }

    }
}
