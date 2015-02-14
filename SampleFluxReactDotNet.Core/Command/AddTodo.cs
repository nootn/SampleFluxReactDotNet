using System;
using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Security;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;

namespace SampleFluxReactDotNet.Core.Command
{
    public class AddTodo : CommandBase<string>, IAddTodo
    {
        public readonly IUser CurrentUserOfApp;
        public readonly IEventStoreProxy EventStore;

        public AddTodo(IUser currentUserOfApp,
            IEventStoreProxy eventStore)
        {
            CurrentUserOfApp = currentUserOfApp;
            EventStore = eventStore;
        }

        public override void Execute(string model)
        {
            var evt = new TodoCreatedEvent
            {
                Author = CurrentUserOfApp.UserId,
                Text = model,
                TodoId = Guid.NewGuid(),
            };
            EventStore.AppendToStream(TodoCreatedEvent.StreamName, evt);
        }
    }
}