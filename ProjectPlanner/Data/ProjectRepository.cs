using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _dataContext;

        public ProjectRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IEnumerable<Project> GetAllProjects(User user)
        {
            //var user2 = await _dataContext.Users.Include(u => u.Projects.Select(p => p.Todos)).FirstOrDefaultAsync(u => u.Id == user.Id);
            var projects = _dataContext.Projects.Where(p => p.UserId == user.Id).Include(p => p.Todos);
            return projects;
        }
        public async Task CreateProject(Project project, User user)
        {
            if(project == null || user == null)
            {
                throw new ArgumentNullException("Project or user cannot be null");
            }
            else
            {
                var newProject = new Project
                {
                    User = user,
                    UserId = user.Id,
                    Name = project.Name,
                    Description = project.Description,
                    CreatedDate = DateTime.Now,
                    EstimatedDate = project.EstimatedDate,
                    PercentageOfCompletion = 0.00M,
                };

                await _dataContext.Projects.AddAsync(newProject);
                await _dataContext.SaveChangesAsync().ConfigureAwait(true);
            }
         
        }

        public async Task DeleteProject(int projectId)
        {
            var project = await GetProjectById(projectId).ConfigureAwait(true);

            if (project != null)
            {
                _dataContext.Remove(project);
                await _dataContext.SaveChangesAsync().ConfigureAwait(true);
            }
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            var project = await _dataContext.Projects.Include(p => p.Todos).FirstOrDefaultAsync(x => x.ProjectId == projectId).ConfigureAwait(true);


            return project;

        }

        public async Task<bool> ProjectExists(int projectId)
        {
            return await _dataContext.Projects.AnyAsync(x => x.ProjectId == projectId).ConfigureAwait(true);
        }

        public async Task UpdateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException();
            }

            var editedProject = await GetProjectById(project.ProjectId).ConfigureAwait(true);

            if (editedProject != null)
            {
                editedProject.Name = project.Name;
                editedProject.Description = project.Description;
                editedProject.EstimatedDate = project.EstimatedDate;

                _dataContext.Projects.Update(editedProject);
                await _dataContext.SaveChangesAsync().ConfigureAwait(true);
            }
        }

        public async Task CalculateCompletion(int projectId)
        {
            var project = await GetProjectById(projectId).ConfigureAwait(true);

            var todos = project.Todos;

            var completedTodos = project.Todos.Where(t => t.Status == "Completed");

            decimal percentage;

            if (completedTodos.Any())
            {
                percentage = (completedTodos.Count() * 100) / todos.Count;

                percentage = Math.Round(percentage, 2);
            }
            else
            {
                percentage = 0.00m;
            }

            project.PercentageOfCompletion = percentage;

            _dataContext.Projects.Update(project);

            await _dataContext.SaveChangesAsync().ConfigureAwait(true);
        }
    }
}
