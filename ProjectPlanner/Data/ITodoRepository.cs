using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public interface ITodoRepository
    {
        public Task<Project> GetProjectById(int projectId);
        public Task<IEnumerable<Todo>> AllTodosByProjectId(int projectId);
        public Task<Todo> GetTodoById(int todoId);

        public Task CreateTodo(int projectId, Todo todo);

        public Task UpdateTodo(Todo todo);

        public Task DeleteTodo(int todoId);

        public Task ChangeTodoStatus(int todoId, string status);

    }
}
