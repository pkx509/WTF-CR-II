using System;

namespace DITS.HILI.WMS.MasterModel
{
    public class BasePrefix
    {
        public Guid PrefixID { get; set; }
        public string PrefixKey { get; set; }
        public string FormatKey { get; set; }
        public int LengthKey { get; set; }
        public string LastedKey { get; set; }
        public bool? IsLastest { get; set; }

    }
}
