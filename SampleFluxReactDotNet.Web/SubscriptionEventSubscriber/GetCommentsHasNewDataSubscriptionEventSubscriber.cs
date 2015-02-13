using DotNetAppStarterKit.Core.Event;
using SampleFluxReactDotNet.Core.SubscriptionEvents;

namespace SampleFluxReactDotNet.Web.SubscriptionEventSubscriber
{
    public class GetCommentsHasNewDataSubscriptionEventSubscriber :
        IEventSubscriber<GetCommentsHasNewDataSubscriptionEvent>
    {
        public void Handle(GetCommentsHasNewDataSubscriptionEvent data)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerEventsHub>();
            //var now = DateTimeOffset.Now;
            //hubContext.Clients.All.CommentsUpdated(now);
        }
    }
}