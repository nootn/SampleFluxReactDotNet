using System;
using System.Linq;
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
        public readonly IAddTodo AddTodoCommand;
        public readonly IGetTodos GetTodosQuery;

        public HomeController(IAddComment addCommentCommand, 
            IGetComments getCommentsQuery,
            IAddTodo addTodoCommand,
            IGetTodos getTodosQuery)
        {
            AddCommentCommand = addCommentCommand;
            GetCommentsQuery = getCommentsQuery;
            AddTodoCommand = addTodoCommand;
            GetTodosQuery = getTodosQuery;
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
        public virtual ActionResult AddComment(string text)
        {
            AddCommentCommand.Execute(text);

            return Content("Success :)");
        }

        public virtual ActionResult Todos()
        {
            var item = GetTodosQuery.ExecuteCached() ?? GetTodosQuery.Execute();
            return Json(item.TodoDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult AddTodo(string text)
        {
            AddTodoCommand.Execute(text);

            return Content("Success :)");
        }

        [HttpPost]
        public virtual ActionResult ToggleTodo(Guid todoId, bool complete)
        {
            //AddTodoCommand.Execute(text);

            return Content("Success :)");
        }
    }
}