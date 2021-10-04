using System;

namespace DITS.HILI.WMS.Core.PackagesModel
{
    //TODO: Language
    public class WMSLanguage
    {
        public Guid LanguageID { get; set; }
        public string Code { get; set; }
        public string ENG { get; set; }
        public string Culture { get; set; }
        public string ShortCulture { get; set; }
        public string Local { get; set; }

    }
}
