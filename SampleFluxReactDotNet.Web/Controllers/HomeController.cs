using System.Web.Mvc;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.Query.Interface;
using SampleFluxReactDotNet.Web.Models;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public partial class HomeController : Controller
    {
        public readonly IAddComment AddCommentCommand;
        public readonly IGetComments GetCommentsQuery;

        public HomeController(IAddComment addCommentCommand, IGetComments getCommentsQuery)
        {
            AddCommentCommand = addCommentCommand;
            GetCommentsQuery = getCommentsQuery;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Comments()
        {
            var item = GetCommentsQuery.ExecuteCached() ?? GetCommentsQuery.Execute();
            return Json(item.CommentDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult AddComment(CommentModel comment)
        {
            AddCommentCommand.Execute(comment.Text);

            ////todo: this will move to when the stream is updated..
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerEventsHub>();
            //var now = DateTimeOffset.Now;
            //hubContext.Clients.All.CommentsUpdated(now);

            return Content("Success :)");
        }
    }
}