using System;
using System.Collections.Generic;

namespace SampleFluxReactDotNet.Core.View.Comment
{
    public class CommentListView
    {
        public CommentListView()
        {
            CommentDetails = new List<CommentDetailView>();
        }

        public Guid LatestCommentEventId { get; set; }

        public List<CommentDetailView> CommentDetails { get; set; }
    }
}