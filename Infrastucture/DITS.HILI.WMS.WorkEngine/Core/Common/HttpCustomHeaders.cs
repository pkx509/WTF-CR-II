using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Master.Core.Common
{

    public class HttpCustomHeaders
    {
        public HttpCustomHeaders() { }
        public int? PageCount { get; set; }
        public int? TotalRecords { get; set; }
        public string Language { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public Guid? UserID { get; set; }

    }
}

