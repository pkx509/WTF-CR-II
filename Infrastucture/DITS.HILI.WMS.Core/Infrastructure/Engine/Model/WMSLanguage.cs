using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core.Infrastructure.Engine.Model
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
