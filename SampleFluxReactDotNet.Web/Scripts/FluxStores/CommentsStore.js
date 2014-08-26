var FluxStores = FluxStores || {};

FluxStores.CommentsStore = (function () {
    var constants = {
        ADD_COMMENT: "ADD_COMMENT",
        LOAD_COMMENTS: "LOAD_COMMENTS",
    };

    var commentsStore = Fluxxor.createStore({
        initialize: function () {
            this.comments = [];

            this.bindActions(
              constants.ADD_COMMENT, this.onAddComment,
              constants.LOAD_COMMENTS, this.onLoadDataFromServer
            );
        },

        onAddComment: function (payload) {
            var data = new FormData();
            data.append('Author', payload.author);
            data.append('Text', payload.text);

            var xhr = new XMLHttpRequest();
            xhr.open('post', '/Home/AddComment', true);
            xhr.onload = function () {
            }.bind(this);
            xhr.send(data);
        },

        onLoadDataFromServer: function () {
            var xhr = new XMLHttpRequest();
            xhr.open('get', '/Home/Comments', true);
            xhr.onload = function () {
                var data = JSON.parse(xhr.responseText);
                this.comments = data;
                this.emit("change");
            }.bind(this);
            xhr.send();
        },

        getState: function () {
            return {
                comments: this.comments
            };
        }
    });

    var actions = {
        addComment: function (author, text) {
            this.dispatch(constants.ADD_COMMENT, { author: author, text: text });
        },
        loadComments: function () {
            this.dispatch(constants.LOAD_COMMENTS, {});
        },
    };

    //Decide what is public
    return {
        actions: actions,
        store: new commentsStore(),
    };
})();