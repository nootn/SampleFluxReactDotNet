using System;
using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Security;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;

namespace SampleFluxReactDotNet.Core.Command
{
    public class ToggleTodo : CommandBase<Tuple<Guid, bool>>, IToggleTodo
    {
        public readonly IUser CurrentUserOfApp;
        public readonly IEventStoreProxy EventStore;

        public ToggleTodo(IUser currentUserOfApp,
            IEventStoreProxy eventStore)
        {
            CurrentUserOfApp = currentUserOfApp;
            EventStore = eventStore;
        }

        public override void Execute(Tuple<Guid, bool> model)
        {
            var evt = new TodoCompleteToggledEvent
            {
                TodoId = model.Item1,
                Complete = model.Item2,
            };
            EventStore.AppendToStream(TodoCompleteToggledEvent.StreamName, evt);
        }
    }
}