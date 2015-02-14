using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAppStarterKit.Core.Query;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.EventStore;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;
using SampleFluxReactDotNet.Core.Query.Interface;
using SampleFluxReactDotNet.Core.View.Todo;

namespace SampleFluxReactDotNet.Core.Query
{
    public class GetTodos : CachedQueryBase<TodoListView>, IGetTodos
    {
        public readonly IEventStoreProxy EventStore;

        public GetTodos(IEventStoreProxy eventStore)
        {
            EventStore = eventStore;
        }

        public override TodoListView Execute()
        {
            var res = new TodoListView();

            var resultDict = new Dictionary<Guid, TodoDetailView>();

            var slice =
                EventStore.ReadStreamEventsForward(TodoCreatedEvent.StreamName, StreamPosition.Start, int.MaxValue);
            foreach (var currEvent in slice.Events)
            {
                var item = ManipulateEvent.DeserializeEvent(currEvent.OriginalEvent.Metadata,
                    currEvent.OriginalEvent.Data);

                if (item != null)
                {
                    //figure out which type it is and set relevant data accordingly
                    if (item is TodoCreatedEvent)
                    {
                        var data = item as TodoCreatedEvent;
                        resultDict.Add(data.TodoId, new TodoDetailView
                        {
                            Author = data.Author,
                            Complete = false,
                            Text = data.Text,
                            TodoId = data.TodoId,
                        });
                    }
                    else if (item is TodoCompleteToggledEvent)
                    {
                        var data = item as TodoCompleteToggledEvent;
                        if (resultDict.ContainsKey(data.TodoId))
                        {
                            resultDict[data.TodoId].Complete = data.Complete;
                        }
                        else
                        {
                            //todo: log it - weird!!
                        }
                    }
                }
            }

            res.TodoDetails = resultDict.Values.OrderBy(_ => _.Text).ToList();

            return res;
        }
    }
}