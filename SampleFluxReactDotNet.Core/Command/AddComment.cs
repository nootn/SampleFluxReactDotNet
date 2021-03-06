﻿using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Security;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.EventStore.Event;
using SampleFluxReactDotNet.Core.EventStore.Interface;

namespace SampleFluxReactDotNet.Core.Command
{
    public class AddComment : CommandBase<string>, IAddComment
    {
        public readonly IUser CurrentUserOfApp;
        public readonly IEventStoreProxy EventStore;

        public AddComment(IUser currentUserOfApp,
            IEventStoreProxy eventStore)
        {
            CurrentUserOfApp = currentUserOfApp;
            EventStore = eventStore;
        }

        public override void Execute(string model)
        {
            var evt = new CommentCreatedEvent
            {
                Author = CurrentUserOfApp.UserId,
                Text = model,
            };
            EventStore.AppendToStream(CommentCreatedEvent.StreamName, evt);
        }
    }
}