using DotNetAppStarterKit.Core.Query;
using SampleFluxReactDotNet.Core.View.Comment;

namespace SampleFluxReactDotNet.Core.Query.Interface
{
    public interface IGetComments : ICachedQuery<CommentListView>
    {
    }
}