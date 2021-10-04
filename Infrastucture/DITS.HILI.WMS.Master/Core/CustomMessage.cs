using System;

namespace DITS.HILI.WMS.MasterModel.Core
{
    public class CustomMessage
    {
        public Guid MessageId { get; set; }
        public string MessageCode { get; set; }
        public string MessageTitle { get; set; }
        public string MessageValue { get; set; }
        public string MessageOrgValue { get; set; }
        public string LanguageCode { get; set; }
        public DateTime DateModified { get; set; }
    }
}
