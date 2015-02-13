using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SampleFluxReactDotNet.Core.Hubs
{
    [HubName("ServerEventsHub")]
    public class ServerEventsHub : Hub
    {
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