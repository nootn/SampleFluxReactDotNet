using System;
using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Security;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.EventStore;
using SampleFluxReactDotNet.Core.EventStore.Stream;

namespace SampleFluxReactDotNet.Core.Command
{
    public class AddComment : CommandBase<string>, IAddComment
    {
        public readonly IUser CurrentUserOfApp;
        public readonly IEventStoreConnection EventStoreConn;

        public AddComment(IUser currentUserOfApp,
            IEventStoreConnection eventStoreConn)
        {
            CurrentUserOfApp = currentUserOfApp;
            EventStoreConn = eventStoreConn;
        }

        public override void Execute(string model)
        {
            EventStoreConn.ConnectAsync().Wait();
            var evt = new CommentCreatedEvent
            {
                Author = CurrentUserOfApp.UserId,
                Text = model,
            };
            var data = ManipulateEvent.ToEventData(Guid.NewGuid(), evt, null);
            EventStoreConn.AppendToStreamAsync(CommentCreatedEvent.StreamName, ExpectedVersion.Any, data).Wait();
        }
    }
}