namespace SampleFluxReactDotNet.Core.EventStore.Event
{
    public class CommentCreatedEvent
    {
        public static readonly string StreamName = Streams.CommentStreamName;

        public string Author { get; set; }
        public string Text { get; set; }
    }
}