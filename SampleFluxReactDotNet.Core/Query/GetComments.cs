using System.Collections.Generic;
using DotNetAppStarterKit.Core.Query;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.EventStore;
using SampleFluxReactDotNet.Core.EventStore.Stream;
using SampleFluxReactDotNet.Core.Query.Interface;
using SampleFluxReactDotNet.Core.View.Comment;

namespace SampleFluxReactDotNet.Core.Query
{
    public class GetComments : CachedQueryBase<CommentListView>, IGetComments
    {
        public readonly IEventStoreConnection EventStoreConn;

        public GetComments(IEventStoreConnection eventStoreConn)
        {
            EventStoreConn = eventStoreConn;
        }

        public override CommentListView Execute()
        {
            var res = new CommentListView
            {
                CommentDetails = new List<CommentDetailView>(),
            };

            EventStoreConn.ConnectAsync().Wait();
            var slice =
                EventStoreConn.ReadStreamEventsForwardAsync(CommentCreatedEvent.StreamName, StreamPosition.Start,
                    int.MaxValue, false).Result;
            foreach (var currEvent in slice.Events)
            {
                var item = ManipulateEvent.DeserializeEvent(currEvent.OriginalEvent.Metadata,
                    currEvent.OriginalEvent.Data) as CommentCreatedEvent;

                if (item != null)
                {
                    res.LatestCommentEventId = currEvent.Event.EventId;

                    res.CommentDetails.Add(new CommentDetailView
                    {
                        Author = item.Author,
                        Id = res.LatestCommentEventId,
                        Text = item.Text,
                    });
                }
            }

            return res;
        }
    }
}