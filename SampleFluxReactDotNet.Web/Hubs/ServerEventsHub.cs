using System;
using System.Threading.Tasks;
using DotNetAppStarterKit.Core.Logging;
using EventStore.ClientAPI;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SampleFluxReactDotNet.Core.EventStore.Stream;

namespace SampleFluxReactDotNet.Web.Hubs
{
    [HubName("ServerEventsHub")]
    public class ServerEventsHub : Hub
    {
        public readonly IEventStoreConnection EventStoreConn;
        public readonly ILog<ServerEventsHub> Logger;

        public ServerEventsHub(IEventStoreConnection eventStoreConn, ILog<ServerEventsHub> logger)
        {
            EventStoreConn = eventStoreConn;
            Logger = logger;

            SubscribeToCommentCreatedEvent();
        }

        private void SubscribeToCommentCreatedEvent()
        {
            EventStoreConn.SubscribeToStreamAsync(CommentCreatedEvent.StreamName, false, NewCommentCreated,
                NewCommentCreatedSubscriptionDropped);
        }

        private void NewCommentCreated(EventStoreSubscription sub, ResolvedEvent evt)
        {
            Logger.Debug("New Comment Created Id: {0}", evt.Event.EventId);
        }

        private void NewCommentCreatedSubscriptionDropped(EventStoreSubscription sub, SubscriptionDropReason reason, Exception ex)
        {
            Logger.Error("New Comment Created Subscription Dropped", ex);

            //maybe try to re-subscribe?
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