namespace SampleFluxReactDotNet.Core.EventStore.Stream
{
    public class CommentCreatedEvent
    {
        public static readonly string StreamName = "SampleFluxReactDotNet-Comment";

        public string Author { get; set; }
        public string Text { get; set; }
    }
}