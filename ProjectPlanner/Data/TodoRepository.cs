using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var project = await _dataContext.Projects.Include(x => x.Todos).FirstOrDefaultAsync(x => x.ProjectId == projectId);

            return project;
        }

        public async Task<IEnumerable<Todo>> AllTodosByProjectId(int projectId)
        {
            var project = await GetProjectById(projectId);

            var todos = project.Todos;

            return todos;
        }

        public async Task CreateTodo(int projectId, Todo todo)
        {
            var project = await GetProjectById(projectId);

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

            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteTodo(int todoId)
        {
            var todo = await GetTodoById(todoId);

            _dataContext.Remove(todo);
            await _dataContext.SaveChangesAsync();
        }

        
        public async Task<Todo> GetTodoById(int todoId)
        {
            var todo = await _dataContext.Todos.FirstOrDefaultAsync(x => x.TodoId == todoId);

            return todo;
        }

        public async Task UpdateTodo(Todo todo)
        {
            var editedTodo = await GetTodoById(todo.TodoId);

            if (editedTodo != null)
            {
                editedTodo.Name = todo.Name;
                editedTodo.Description = todo.Description;
                editedTodo.EstimatedDate = todo.EstimatedDate;

                _dataContext.Todos.Update(editedTodo);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task ChangeTodoStatus(int todoId, string status)
        {
            var todo = await GetTodoById(todoId);

            todo.Status = status;

            _dataContext.Todos.Update(todo);
            await _dataContext.SaveChangesAsync();
        }
       
    }
}
