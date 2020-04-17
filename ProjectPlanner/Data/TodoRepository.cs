using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _dataContext;

        public TodoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            var project = await _dataContext.Projects.Include(x => x.Todos).FirstOrDefaultAsync(x => x.ProjectId == projectId).ConfigureAwait(true);

            return project;
        }

        public async Task<IEnumerable<Todo>> AllTodosByProjectId(int projectId)
        {
            var project = await GetProjectById(projectId).ConfigureAwait(true);

            var todos = project.Todos;

            return todos;
        }

        public async Task CreateTodo(int projectId, Todo todo)
        {
            if(projectId == 0 || todo == null)
            {
                throw new ArgumentNullException();
            }

            var project = await GetProjectById(projectId).ConfigureAwait(true);

            var newTodo = new Todo
            {
                Name = todo.Name,
                Description = todo.Description,
                CreatedDate = DateTime.Now,
                EstimatedDate = todo.EstimatedDate,
                Status = "Pending",
            };

            project.Todos.Add(newTodo);

            _dataContext.Projects.Update(project);

            await _dataContext.SaveChangesAsync().ConfigureAwait(true);
        }

        public async Task DeleteTodo(int todoId)
        {
            var todo = await GetTodoById(todoId).ConfigureAwait(true);

            _dataContext.Remove(todo);
            await _dataContext.SaveChangesAsync().ConfigureAwait(true);
        }


        public async Task<Todo> GetTodoById(int todoId)
        {
            var todo = await _dataContext.Todos.FirstOrDefaultAsync(x => x.TodoId == todoId).ConfigureAwait(true);

            return todo;
        }

        public async Task UpdateTodo(Todo todo)
        {   
            if (todo == null)
            {
                throw new ArgumentNullException();
            }

            var editedTodo = await GetTodoById(todo.TodoId).ConfigureAwait(true);

            if (editedTodo != null)
            {
                editedTodo.Name = todo.Name;
                editedTodo.Description = todo.Description;
                editedTodo.EstimatedDate = todo.EstimatedDate;

                _dataContext.Todos.Update(editedTodo);
                await _dataContext.SaveChangesAsync().ConfigureAwait(true);
            }
        }

        public async Task ChangeTodoStatus(int todoId, string status)
        {
            var todo = await GetTodoById(todoId).ConfigureAwait(true);

            todo.Status = status;

            _dataContext.Todos.Update(todo);
            await _dataContext.SaveChangesAsync().ConfigureAwait(true);
        }

    }
}
