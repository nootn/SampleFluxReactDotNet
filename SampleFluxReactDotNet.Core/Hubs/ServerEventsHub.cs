using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SampleFluxReactDotNet.Core.Hubs
{
    [HubName("ServerEventsHub")]
    public class ServerEventsHub : Hub
    {
        public static void CallClientCommentsUpdated()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerEventsHub>();
            var now = DateTimeOffset.Now;
            hubContext.Clients.All.CommentsUpdated(now);
        }

        public override Task OnConnected()
        {
            CallClientCommentsUpdated();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            CallClientCommentsUpdated();
            return base.OnReconnected();
        }
    }
}