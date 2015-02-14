using System;

namespace SampleFluxReactDotNet.Core.EventStore.Event
{
    public class TodoCompleteToggledEvent
    {
        public static readonly string StreamName = Streams.TodoStreamName;

        public Guid TodoId { get; set; }
        public bool Complete { get; set; }
    }
}