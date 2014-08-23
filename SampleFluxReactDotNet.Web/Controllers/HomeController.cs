using System.Collections.Generic;
using System.Web.Mvc;
using SampleFluxReactDotNet.Web.Models;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comments()
        {
            var comments = new List<CommentModel>
            {
                new CommentModel
                {
                    Author = "Daniel Lo Nigro",
                    Text = "Hello ReactJS.NET World!"
                },
                new CommentModel
                {
                    Author = "Pete Hunt",
                    Text = "This is one comment"
                },
                new CommentModel
                {
                    Author = "Jordan Walke",
                    Text = "This is *another* comment"
                },
            };

            return Json(comments, JsonRequestBehavior.AllowGet);
        }
    }
}