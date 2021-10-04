using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DITS.HILI.WMS.Core
{
    public class WmsHub : Hub
    {
        private static Dictionary<string, string> users;

        public override Task OnConnected()
        {
            if (users == null)
            {
                users = new Dictionary<string, string>();
            }

            users.Add("", "");
            return base.OnConnected();
        }
    }
}
