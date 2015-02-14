using System;
using System.Collections.Generic;

namespace SampleFluxReactDotNet.Core.View.Comment
{
    public class CommentListView
    {
        public Guid LatestCommentEventId { get; set; }

        public List<CommentDetailView> CommentDetails { get; set; }
    }
}