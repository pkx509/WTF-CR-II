using System;
using System.Linq;
using System.Threading.Tasks;
using DITS.FoodTruckQueue.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DITS.FoodTruckQueue.Web.Hubs
{
    public class QueueHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private static IHubConnectionContext<dynamic> GetClients(QueueHub queueHub)
        {
            if (queueHub == null)
                return GlobalHost.ConnectionManager.GetHubContext<QueueHub>().Clients;
            else
                return queueHub.Clients;
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            _connections.Add(name, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }
            return base.OnReconnected();
        }

        //public void SendChatMessage(string who, string message)
        //{
        //    string name = Context.User.Identity.Name;
        //    foreach (var connectionId in _connections.GetConnections(who))
        //    {
        //        Clients.Client(connectionId).addChatMessage(name + ": " + message);
        //    }
        //}

        public static void SentCallQueue(string file, QueueHub queueHub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(queueHub);
            clients.All.CallQueue(file); 
        }
        public static void SentCompletedQueue(string file, QueueHub queueHub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(queueHub);
            clients.All.CompletedQueue(file); 
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        //public void Send(string channel, string content)
        //{
        //    Clients.Group(channel).addMessage(content);
        //}
        //public static void Send(string message, QueueHub queueHub)
        //{
        //    IHubConnectionContext<dynamic> clients = GetClients(queueHub);
        //    clients.All.receiveSend(message);
        //}
        public static void ChangedAnounce(string message, bool enable, QueueHub queueHub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(queueHub);
            clients.All.ChangedAnounce(message, enable);
        }

        public static void RefreshQueue(QueueHub queueHub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(queueHub);
            clients.All.RefreshQueue("Refresh");
        }
    }
}