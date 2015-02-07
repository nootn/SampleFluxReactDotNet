using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SampleFluxReactDotNet.Web.Hubs;
using SampleFluxReactDotNet.Web.Models;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private static List<CommentModel> _comments = new List<CommentModel>() { new CommentModel { Author = "[unknown]", Text = "The *first* comment!", Id = Guid.NewGuid().ToString()} };

        public virtual ActionResult Index()
        {
            return View(_comments);
        }

        public virtual ActionResult Comments()
        {
            return Json(_comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult AddComment(CommentModel comment)
        {
            comment.Id = Guid.NewGuid().ToString();
            _comments.Add(comment);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ServerEventsHub>();
            var now = DateTimeOffset.Now;
            hubContext.Clients.All.CommentsUpdated(now);

            return Content("Success :)");
        }
    }
}