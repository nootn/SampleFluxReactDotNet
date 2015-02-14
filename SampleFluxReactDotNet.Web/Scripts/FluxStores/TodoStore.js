var FluxStores = FluxStores || {};

FluxStores.TodoStore = (function() {
    var constants = {
        ADD_TODO: "ADD_TODO",
        TOGGLE_TODO: "TOGGLE_TODO",
        CLEAR_TODOS: "CLEAR_TODOS",
        LOAD_TODOS: "LOAD_TODOS",
    };

    var todoStore = Fluxxor.createStore({
        initialize: function () {
            this.todos = [];

            this.bindActions(
              constants.ADD_TODO, this.onAddTodo,
              constants.TOGGLE_TODO, this.onToggleTodo,
              constants.CLEAR_TODOS, this.onClearTodos,
              constants.LOAD_TODOS, this.onLoadDataFromServer
            );
        },

        onAddTodo: function (payload) {
            var data = new FormData();
            data.append('text', payload.text);

            var xhr = new XMLHttpRequest();
            xhr.open('post', '/Home/AddTodo', true);
            xhr.onload = function () {
            }.bind(this);
            xhr.send(data);
        },

        onToggleTodo: function (payload) {
            var data = new FormData();
            data.append('todoId', payload.todo.TodoId);
            data.append('complete', !payload.todo.Complete);

            var xhr = new XMLHttpRequest();
            xhr.open('post', '/Home/ToggleTodo', true);
            xhr.onload = function () {
            }.bind(this);
            xhr.send(data);
        },

        onClearTodos: function () {
            var completedIds = [];
            for (var i = 0; i < this.todos.length; i++) {
                var currTodo = this.todos[i];
                if (currTodo.Complete) {
                    completedIds.push(currTodo.TodoId);
                }
            }

            var data = new FormData();
            data.append('todoIds', completedIds);

            var xhr = new XMLHttpRequest();
            xhr.open('post', '/Home/ClearCompletedTodos', true);
            xhr.onload = function () {
            }.bind(this);
            xhr.send(data);
        },

        onLoadDataFromServer: function () {
            var xhr = new XMLHttpRequest();
            xhr.open('get', '/Home/Todos', true);
            xhr.onload = function () {
                var data = JSON.parse(xhr.responseText);
                this.todos = data;
                this.emit("change");
            }.bind(this);
            xhr.send();
        },

        getState: function () {
            return {
                todos: this.todos
            };
        }
    });

    var actions = {
        addTodo: function (text) {
            this.dispatch(constants.ADD_TODO, { text: text });
        },

        toggleTodo: function (todo) {
            this.dispatch(constants.TOGGLE_TODO, { todo: todo });
        },

        clearTodos: function () {
            this.dispatch(constants.CLEAR_TODOS);
        },
        loadTodos: function () {
            this.dispatch(constants.LOAD_TODOS, {});
        },
    };

    //Decide what is public
    return {
        actions: actions,
        store: new todoStore(),
    };
})();