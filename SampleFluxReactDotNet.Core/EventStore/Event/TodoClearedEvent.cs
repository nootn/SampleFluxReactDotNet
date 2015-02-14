using System;

namespace SampleFluxReactDotNet.Core.EventStore.Event
{
    public class TodoClearedEvent
    {
        public static readonly string StreamName = Streams.TodoStreamName;

        public Guid TodoId { get; set; }
    }
}