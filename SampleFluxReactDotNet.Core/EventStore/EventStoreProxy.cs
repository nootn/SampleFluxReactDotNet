using System;
using System.Net;
using Autofac;
using DotNetAppStarterKit.Core.Event;
using EventStore.ClientAPI;
using Microsoft.AspNet.SignalR;
using SampleFluxReactDotNet.Core.EventStore.Interface;
using SampleFluxReactDotNet.Core.EventStore.Stream;
using SampleFluxReactDotNet.Core.Hubs;
using SampleFluxReactDotNet.Core.SubscriptionEvents;

namespace SampleFluxReactDotNet.Core.EventStore
{
    public class EventStoreProxy : IEventStoreProxy
    {
        private readonly IComponentContext _container;
        private readonly IEventPublisher<GetCommentsHasNewDataSubscriptionEvent> _getCommentsHasNewDataSubscriptionPublisher;
        private static IEventStoreConnection _eventStoreConn;
        private static readonly object CreateConnectionLock = new object();

        //public EventStoreProxy(IEventPublisher<GetCommentsHasNewDataSubscriptionEvent> getCommentsHasNewDataSubscriptionPublisher)
        public EventStoreProxy(IComponentContext container)
        {
            _container = container;
            //_getCommentsHasNewDataSubscriptionPublisher = getCommentsHasNewDataSubscriptionPublisher;
            //Ensure we only set up the connection once
            lock (CreateConnectionLock)
            {
                if (_eventStoreConn == null)
                {
                    var connSettings = ConnectionSettings.Create()
                        .KeepReconnecting()
                        .KeepRetrying();

                    //TODO: get config value for address, port and user account
                    _eventStoreConn = EventStoreConnection.Create(connSettings, new IPEndPoint(IPAddress.Loopback, 1113));
                    _eventStoreConn.Disconnected += EventStoreConnDisconnected;
                    _eventStoreConn.ErrorOccurred += EventStoreConnErrorOccurred;
                    _eventStoreConn.Reconnecting += EventStoreConnReconnecting;
                    _eventStoreConn.Connected += EventStoreConnConnected;
                    _eventStoreConn.ConnectAsync().Wait();

                    SubscribeToStreamNewComment();
                }
            }
        }

        public Guid AppendToStream(string streamName, object evnt)
        {
            var id = Guid.NewGuid();
            var data = ManipulateEvent.ToEventData(id, evnt, null);
            _eventStoreConn.AppendToStreamAsync(streamName, ExpectedVersion.Any, data).Wait();
            return id;
        }

        public StreamEventsSlice ReadStreamEventsForward(string streamName, int start, int count)
        {
            return _eventStoreConn.ReadStreamEventsForwardAsync(streamName, start, count, false).Result;
        }

        private void SubscribeToStreamNewComment()
        {
            _eventStoreConn.SubscribeToStreamAsync(CommentCreatedEvent.StreamName, false,
                NewCommentCreated,
                NewCommentCreatedSubscriptionDropped);
        }

        private void NewCommentCreated(EventStoreSubscription sub, ResolvedEvent evt)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerEventsHub>();
            var now = DateTimeOffset.Now;
            hubContext.Clients.All.CommentsUpdated(now);
        }

        private void NewCommentCreatedSubscriptionDropped(EventStoreSubscription sub, SubscriptionDropReason reason, Exception ex)
        {
            //TODO: maybe try to re-subscribe?  not sure how long to wait, whether we need multiple retries etc.. also log it..
            //System.Threading.Thread.Sleep(2000);
            //SubscribeToStreamNewComment();
        }

        private static void EventStoreConnConnected(object sender, ClientConnectionEventArgs e)
        {
        }

        private static void EventStoreConnReconnecting(object sender, ClientReconnectingEventArgs e)
        {
        }

        private static void EventStoreConnErrorOccurred(object sender, ClientErrorEventArgs e)
        {
        }

        private static void EventStoreConnDisconnected(object sender, ClientConnectionEventArgs e)
        {
        }
    }
}