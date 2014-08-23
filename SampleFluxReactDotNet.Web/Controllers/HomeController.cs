using System.Collections.Generic;
using System.Web.Mvc;
using SampleFluxReactDotNet.Web.Models;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public class HomeController : Controller
    {
        private static List<CommentModel> _comments = new List<CommentModel>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comments()
        {
            return Json(_comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            _comments.Add(comment);
            return Content("Success :)");
        }
    }
}