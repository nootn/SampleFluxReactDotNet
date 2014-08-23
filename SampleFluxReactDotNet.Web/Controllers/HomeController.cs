using System.Collections.Generic;
using System.Web.Mvc;
using SampleFluxReactDotNet.Web.Models;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private static List<CommentModel> _comments = new List<CommentModel>() { new CommentModel { Author = "[unknown]", Text = "The *first* comment!" } };

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
            _comments.Add(comment);
            return Content("Success :)");
        }
    }
}