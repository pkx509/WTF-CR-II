using DITS.HILI.WMS.Master.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DITS.HILI.WMS.APIs
{
    public class BaseApiController : ApiController
    { 
        protected HttpCustomHeaders _header;
    }
}