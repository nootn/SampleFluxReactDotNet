using System;
using Microsoft.AspNet.SignalR;

namespace SampleFluxReactDotNet.Web.Hubs
{
    public class CommentsHub : Hub
    {
        public void CommentsUpdated(DateTimeOffset at)
        {
            Clients.All.CommentsUpdated(at);
        }
    }
}