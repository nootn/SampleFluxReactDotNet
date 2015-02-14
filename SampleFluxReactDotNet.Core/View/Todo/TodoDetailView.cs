using System;

namespace SampleFluxReactDotNet.Core.View.Todo
{
    public class TodoDetailView
    {
        public Guid TodoId { get; set; }

        public string Author { get; set; }

        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}