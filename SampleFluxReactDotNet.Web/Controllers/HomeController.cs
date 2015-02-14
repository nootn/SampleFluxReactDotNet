using System;
using System.Linq;
using System.Web.Mvc;
using SampleFluxReactDotNet.Core.Command.Interface;
using SampleFluxReactDotNet.Core.Query.Interface;

namespace SampleFluxReactDotNet.Web.Controllers
{
    public partial class HomeController : Controller
    {
        public readonly IAddComment AddCommentCommand;
        public readonly IAddTodo AddTodoCommand;
        public readonly IGetComments GetCommentsQuery;
        public readonly IGetTodos GetTodosQuery;
        public readonly IToggleTodo ToggleTodoCommand;
        public readonly IClearTodos ClearTodosCommand;

        public HomeController(IAddComment addCommentCommand,
            IGetComments getCommentsQuery,
            IAddTodo addTodoCommand,
            IGetTodos getTodosQuery,
            IToggleTodo toggleTodoCommand,
            IClearTodos clearTodosCommand)
        {
            AddCommentCommand = addCommentCommand;
            GetCommentsQuery = getCommentsQuery;
            AddTodoCommand = addTodoCommand;
            GetTodosQuery = getTodosQuery;
            ToggleTodoCommand = toggleTodoCommand;
            ClearTodosCommand = clearTodosCommand;
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
            //Need to figure out validation
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ApplicationException("Must supply text");
            }

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
            //Need to figure out validation
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ApplicationException("Must supply text");
            }

            AddTodoCommand.Execute(text);

            return Content("Success :)");
        }

        [HttpPost]
        public virtual ActionResult ToggleTodo(string todoId, bool complete)
        {
            //Need to figure out validation
            if (string.IsNullOrWhiteSpace(todoId))
            {
                throw new ApplicationException("Unable to determine todoId");
            }
            var id = Guid.Parse(todoId);

            ToggleTodoCommand.Execute(new Tuple<Guid, bool>(id, complete));

            return Content("Success :)");
        }

        [HttpPost]
        public virtual ActionResult ClearCompletedTodos(string todoIds)
        {
            if (!string.IsNullOrWhiteSpace(todoIds))
            {
                var ids = todoIds.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    ClearTodosCommand.Execute(ids.Select(Guid.Parse).ToArray());
                }
            }

            return Content("Success :)");
        }
    }
}