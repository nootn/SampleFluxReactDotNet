using System;
using EventStore.ClientAPI;

namespace SampleFluxReactDotNet.Core.EventStore.Interface
{
    public interface IEventStoreProxy
    {
        Guid AppendToStream(string streamName, object evnt);

        StreamEventsSlice ReadStreamEventsForward(string streamName, int start, int count);
    }
}