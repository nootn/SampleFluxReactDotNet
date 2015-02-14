$(function () {

    //Handle server calls
    var hub = $.connection.ServerEventsHub;
    hub.client.commentsUpdated = function (at) {
        console.log("commentsUpdated called from server");
        Jsx.Comments.flux.actions.loadComments();
    };
    hub.client.todosUpdated = function (at) {
        console.log("todosUpdated called from server");
        Jsx.Todo.flux.actions.loadTodos();
    };

    //Connection events
    $.connection.hub.connectionSlow(function () {
        console.log("Hub connection slow");
    });

    $.connection.hub.reconnecting(function () {
        console.log("Hub reconnecting");
    });

    $.connection.hub.reconnected(function () {
        console.log("Hub reconnected");
    });

    $.connection.hub.disconnected(function () {
        console.log("Hub disconnected");
        if ($.connection.hub.lastError) {
            alert(" Disconnected Reason: " + $.connection.hub.lastError.message);
        }
        setTimeout(function () {
            $.connection.hub.start.done(function () {
                console.log("Hub started after retry");
            });
        }, 5000); // Restart connection after 5 seconds.
    });

    $.connection.hub.start().done(function () {
        console.log("Hub started initially");
    });

});
