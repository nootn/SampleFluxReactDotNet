﻿/** @jsx React.DOM */

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
	handleSubmit: function() {
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
	loadCommentsFromServer: function() {
		var xhr = new XMLHttpRequest();
		xhr.open('get', this.props.url, true);
		xhr.onload = function() {
			var data = JSON.parse(xhr.responseText);
			this.setState({ data: data });
		}.bind(this);
		xhr.send();
	},
	handleCommentSubmit: function(comment) {
		
		var data = new FormData();
		data.append('Author', comment.author);
		data.append('Text', comment.text);

		var xhr = new XMLHttpRequest();
		xhr.open('post', this.props.submitUrl, true);
		xhr.onload = function() {
			this.loadCommentsFromServer();
		}.bind(this);
		xhr.send(data);
	},
	getInitialState: function() {
	    return {data: []};
	},
	componentWillMount: function() {
	    this.loadCommentsFromServer();
	    window.setInterval(this.loadCommentsFromServer, this.props.pollInterval);
	},
	render: function() {
		return (
		  <div className="commentBox">
			<h1>Comments</h1>
			<CommentList data={this.state.data} />
			<CommentForm onCommentSubmit={this.handleCommentSubmit} />
		  </div>
		);
	}
});

React.renderComponent(
  <CommentBox url="/Home/Comments" submitUrl="/Home/AddComment" pollInterval={2000} />,
  document.getElementById('comments')
);