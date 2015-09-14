using System.Collections.Generic;
using DotNetAppStarterKit.Core.Query;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.EventStore;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;
using SampleFluxReactDotNet.Core.Query.Interface;
using SampleFluxReactDotNet.Core.View.Comment;

namespace SampleFluxReactDotNet.Core.Query
{
    public class GetComments : CachedQueryBase<CommentListView>, IGetComments
    {
        public readonly IEventStoreProxy EventStore;

        public GetComments(IEventStoreProxy eventStore)
        {
            EventStore = eventStore;
        }

        public override CommentListView Execute()
        {
            var res = new CommentListView
            {
                CommentDetails = new List<CommentDetailView>(),
            };

            var slice =
                EventStore.ReadStreamEventsForward(Streams.CommentStreamName, StreamPosition.Start);
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