using System;
using System.Net;
using Autofac;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;
using SampleFluxReactDotNet.Core.Hubs;

namespace SampleFluxReactDotNet.Core.EventStore
{
    public class EventStoreProxy : IEventStoreProxy
    {
        private static IEventStoreConnection _eventStoreConn;
        private static readonly object CreateConnectionLock = new object();
        private readonly IComponentContext _container;

        public EventStoreProxy(IComponentContext container)
        {
            _container = container;

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

                    SubscribeToStreamComment();
                    SubscribeToStreamTodo();
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

        private void SubscribeToStreamComment()
        {
            _eventStoreConn.SubscribeToStreamAsync(Streams.CommentStreamName, false,
                CommentChanged,
                CommentChangedSubscriptionDropped);
        }

        private void CommentChanged(EventStoreSubscription sub, ResolvedEvent evt)
        {
            ServerEventsHub.CallClientCommentsUpdated();
        }

        private void CommentChangedSubscriptionDropped(EventStoreSubscription sub, SubscriptionDropReason reason,
            Exception ex)
        {
            SubscribeToStreamComment();
        }

        private void SubscribeToStreamTodo()
        {
            _eventStoreConn.SubscribeToStreamAsync(Streams.TodoStreamName, false,
                TodoChanged,
                TodoChangedSubscriptionDropped);
        }

        private void TodoChanged(EventStoreSubscription sub, ResolvedEvent evt)
        {
            ServerEventsHub.CallClientTodosUpdated();
        }

        private void TodoChangedSubscriptionDropped(EventStoreSubscription sub, SubscriptionDropReason reason,
            Exception ex)
        {
            SubscribeToStreamTodo();
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