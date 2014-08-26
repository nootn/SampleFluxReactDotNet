$(function () {

    //Handle comments
    var hub = $.connection.ServerEventsHub;
    hub.client.commentsUpdated = function (at) {
        Jsx.Comments.flux.actions.loadComments();
    };

    $.connection.hub.start().done(function () {
    });

});
