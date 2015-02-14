using System;
using System.Collections.Generic;
using EventStore.ClientAPI;

namespace SampleFluxReactDotNet.Core.EventStore.Interface
{
    public interface IEventStoreProxy
    {
        Guid AppendToStream(string streamName, object evnt);

        ICollection<Guid> AppendMultipleToStream(string streamName, ICollection<object> evnts);

        StreamEventsSlice ReadStreamEventsForward(string streamName, int start, int count);
    }
}