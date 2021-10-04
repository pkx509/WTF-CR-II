using System;

namespace DITS.HILI.WMS.MasterModel.Core
{
    public class CustomResource
    {
        public Guid ResourceId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public string LanguageCode { get; set; }
        public DateTime DateModified { get; set; }
    }
}
