using System;

namespace SampleFluxReactDotNet.Core.EventStore.Event
{
    public class TodoCreatedEvent
    {
        public static readonly string StreamName = Streams.TodoStreamName;

        public Guid TodoId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}