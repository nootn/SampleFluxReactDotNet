/** @jsx React.DOM */
var Jsx = Jsx || {};

Jsx.Todo = (function () {

    var actions = {
        addTodo: FluxStores.TodoStore.actions.addTodo,
        toggleTodo: FluxStores.TodoStore.actions.toggleTodo,
        clearTodos: FluxStores.TodoStore.actions.clearTodos,
        loadTodos: FluxStores.TodoStore.actions.loadTodos,
    };

    var stores = {
        TodoStore: FluxStores.TodoStore.store,
    };

    var flux = new Fluxxor.Flux(stores, actions);
    var FluxMixin = Fluxxor.FluxMixin(React),
        FluxChildMixin = Fluxxor.FluxChildMixin(React),
        StoreWatchMixin = Fluxxor.StoreWatchMixin;

    var TodoList = React.createClass({
        mixins: [FluxMixin, StoreWatchMixin("TodoStore")],

        getInitialState: function() {
            return { newTodoText: "" };
        },

        getStateFromFlux: function() {
            var flux = this.getFlux();
            // Our entire state is made up of the TodoStore data. In a larger
            // application, you will likely return data from multiple stores, e.g.:
            //
            //   return {
            //     todoData: flux.store("TodoStore").getState(),
            //     userData: flux.store("UserStore").getData(),
            //     fooBarData: flux.store("FooBarStore").someMoreData()
            //   };
            return flux.store("TodoStore").getState();
        },

        render: function() {
            return (
              <div>
                <h1>Todos</h1>
                <ul>
                  {this.state.todos.map(function(todo, i) {
                      return <li key={i}><TodoItem todo={todo} /></li>;
                  })}
                </ul>
                <form onSubmit={this.onSubmitForm}>
                <input type="text" size="30" placeholder="New Todo"
                     value={this.state.newTodoText}
                     onChange={this.handleTodoTextChange} />
              <input type="submit" value="Add Todo" />
            </form>
            <button onClick={this.clearCompletedTodos}>Clear Completed</button>
          </div>
        );
        },

        handleTodoTextChange: function(e) {
            this.setState({newTodoText: e.target.value});
        },

        onSubmitForm: function(e) {
            e.preventDefault();
            if (this.state.newTodoText.trim()) {
                this.getFlux().actions.addTodo(this.state.newTodoText);
                this.setState({newTodoText: ""});
            }
        },

        clearCompletedTodos: function(e) {
            this.getFlux().actions.clearTodos();
        }
    });

    var TodoItem = React.createClass({
        mixins: [FluxChildMixin],

        propTypes: {
            todo: React.PropTypes.object.isRequired
        },

        render: function() {
            var style = {
                textDecoration: this.props.todo.Complete ? "line-through" : ""
            };

            return <span style={style} onClick={this.onClick}>{this.props.todo.Text}</span>;
        },

        onClick: function() {
            this.getFlux().actions.toggleTodo(this.props.todo);
        }
    });

    React.renderComponent(<TodoList flux={flux} />, document.getElementById("todoList"));

    //Decide what is public
    return {
        flux: flux,
    };
})();
