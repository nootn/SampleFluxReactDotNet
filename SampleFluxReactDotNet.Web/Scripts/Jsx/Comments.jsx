/** @jsx React.DOM */
var Jsx = Jsx || {};

Jsx.Comments = (function () {

    var actions = {
        addComment: FluxStores.CommentsStore.actions.addComment,
        loadComments: FluxStores.CommentsStore.actions.loadComments,
    };

    var stores = {
        CommentsStore: FluxStores.CommentsStore.store,
    };

    var flux = new Fluxxor.Flux(stores, actions);
    var FluxMixin = Fluxxor.FluxMixin(React),
        FluxChildMixin = Fluxxor.FluxChildMixin(React),
        StoreWatchMixin = Fluxxor.StoreWatchMixin;

    var converter = new Showdown.converter();

    var Comment = React.createClass({
        render: function() {
            var rawMarkup = this.props.children ? converter.makeHtml(this.props.children.toString()) : "";
            return (
		      <div className="comment">
			    <h2 className="commentAuthor">
			      {this.props.author}
			    </h2>
			    <span dangerouslySetInnerHTML={{__html: rawMarkup}} />
		      </div>
		    );
    }
    });

    var CommentList = React.createClass({
        render: function() {
            var commentNodes = this.props.data.map(function (comment) {
                return <Comment key={comment.Id} author={comment.Author}>{comment.Text}</Comment>;
            });
            return (
			    <div className="commentList">
				    {commentNodes}
			    </div>
		    );
        }
    });

    var CommentForm = React.createClass({
        handleSubmit: function(e) {
            e.preventDefault();
            var author = this.refs.author.getDOMNode().value.trim();
            var text = this.refs.text.getDOMNode().value.trim();
            if (!text || !author) {
                return false;
            }
            this.props.onCommentSubmit({author: author, text: text});
            this.refs.author.getDOMNode().value = '';
            this.refs.text.getDOMNode().value = '';
            return false;
        },
        render: function() {
            return (
			    <form className="commentForm" onSubmit={this.handleSubmit}>
			    <input type="text" placeholder="Your name" ref="author" />
			    <input type="text" placeholder="Say something..." ref="text" />
			    <input type="submit" value="Post" />
			    </form>
		    );
    }
    });

    var CommentBox = React.createClass({
        mixins: [FluxMixin, StoreWatchMixin("CommentsStore")],
        handleCommentSubmit: function(comment) {
            this.getFlux().actions.addComment(comment.author, comment.text);
        },
        getInitialState: function() {
            return {};
        },
        getStateFromFlux: function() {
            var flux = this.getFlux();
            return flux.store("CommentsStore").getState();
        },
        reload: function(){
            this.getFlux().actions.loadComments();
        },
        componentWillMount: function() {
            //this.getFlux().actions.loadComments();
            //window.setInterval(this.getFlux().actions.loadComments, this.props.pollInterval);
        },
        render: function() {
            return (
		      <div className="commentBox">
			    <h1>Comments</h1>
			    <CommentList data={this.state.comments} />
			    <CommentForm onCommentSubmit={this.handleCommentSubmit} />
		      </div>
		    );
        }
    });

    React.renderComponent(
      <CommentBox flux={flux} pollInterval={2000} />,
      document.getElementById('comments')
    );

    //Decide what is public
    return {
        flux: flux,
    };
})();