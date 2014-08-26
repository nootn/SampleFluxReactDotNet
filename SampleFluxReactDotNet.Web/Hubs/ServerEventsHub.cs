using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SampleFluxReactDotNet.Web.Hubs
{
    [HubName("ServerEventsHub")]
    public class ServerEventsHub : Hub
    {
        public void CommentsUpdated()
        {
            var now = DateTimeOffset.Now;

            //Tell the caller first so they get the update as quick as possible
            Clients.Caller.CommentsUpdated(DateTimeOffset.Now);

            //Then tell all the others
            Clients.Others.CommentsUpdated(DateTimeOffset.Now);
        }

        public void GetLatestCommentsOnConnectAndReconnect()
        {
            Clients.Caller.CommentsUpdated(DateTimeOffset.Now);
        }

        public override Task OnConnected()
        {
            GetLatestCommentsOnConnectAndReconnect();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            GetLatestCommentsOnConnectAndReconnect();
            return base.OnReconnected();
        }
    }
}