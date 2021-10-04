using System.Web;
using System.Web.SessionState;

namespace DITS.HILI.WMS.Web.apps
{
    /// <summary>
    /// Summary description for InboundService
    /// </summary>
    public class InboundService : HttpTaskAsyncHandler, IRequiresSessionState
    {
        public override async System.Threading.Tasks.Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
        }
    }
}