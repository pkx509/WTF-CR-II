using System;

namespace DITS.HILI.WMS.Core.ServiceAPIs
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
        public Guid? ProductOwnerID { get; set; }

    }
}

