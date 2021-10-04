namespace DITS.HILI.WMS.DispatchModel
{
    public class DispatchPrefix
    {
        public System.Guid PrefixId { get; set; } // PrefixID (Primary key)
        public string PrefixKey { get; set; } // PrefixKey (length: 20)
        public string FormatKey { get; set; } // FormatKey (length: 20)
        public int LengthKey { get; set; } // LengthKey (Primary key)
        public string LastedKey { get; set; } // LastedKey (length: 20)
        public DispatchPreFixTypeEnum PrefixType { get; set; } // LastedKey (length: 20)

    }
}
