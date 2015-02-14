using System;
using System.Linq;
using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Security;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;

namespace SampleFluxReactDotNet.Core.Command
{
    public class ClearTodos : CommandBase<Guid[]>, IClearTodos
    {
        public readonly IUser CurrentUserOfApp;
        public readonly IEventStoreProxy EventStore;

        public ClearTodos(IUser currentUserOfApp,
            IEventStoreProxy eventStore)
        {
            CurrentUserOfApp = currentUserOfApp;
            EventStore = eventStore;
        }

        public override void Execute(Guid[] model)
        {
            EventStore.AppendMultipleToStream(TodoCompleteToggledEvent.StreamName,
                model.Select(currId => new TodoClearedEvent
                {
                    TodoId = currId,
                }).ToArray());
        }
    }
}