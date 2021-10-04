using System;

namespace DITS.HILI.WMS.MasterModel.Core
{
    public class Language
    {
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string CultureCode { get; set; }
        public string Flag { get; set; }

        public bool IsDefault { get; set; }
        public DateTime DateModified { get; set; }
    }
}
