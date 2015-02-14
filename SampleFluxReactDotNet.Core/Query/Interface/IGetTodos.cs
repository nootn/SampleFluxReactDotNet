using DotNetAppStarterKit.Core.Query;
using SampleFluxReactDotNet.Core.View.Todo;

namespace SampleFluxReactDotNet.Core.Query.Interface
{
    public interface IGetTodos : ICachedQuery<TodoListView>
    {
    }
}