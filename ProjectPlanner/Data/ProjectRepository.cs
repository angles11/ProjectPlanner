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
        public IEnumerable<Project> AllProjects
        {
            get
            {
                return _dataContext.Projects;
            }
        }

        public async Task CreateProject(Project project)
        {
            var newProject = new Project
            {
                Name = project.Name,
                Description = project.Description,
                CreatedDate = DateTime.Now,
                EstimatedDate = project.EstimatedDate,
                PercentageOfCompletion = 0.00M,
            };           

            await _dataContext.Projects.AddAsync(newProject);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteProject(int projectId)
        {
            var project = await GetProjectById(projectId);

            if (project != null)
            {
                _dataContext.Remove(project);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<Project> GetProjectById(int projectId)
        {
            var project = await _dataContext.Projects.Include(p => p.Todos).FirstOrDefaultAsync(x => x.ProjectId == projectId);
                        
            return project;

        }   

        public  async Task<bool> ProjectExists(int projectId)
        {
            return await _dataContext.Projects.AnyAsync(x => x.ProjectId == projectId);
        }

        public async Task UpdateProject(Project project)
        {
            var editedProject = await GetProjectById(project.ProjectId);

            if (editedProject != null)
            {
                editedProject.Name = project.Name;
                editedProject.Description = project.Description;
                editedProject.EstimatedDate = project.EstimatedDate;

                 _dataContext.Projects.Update(editedProject);
                await _dataContext.SaveChangesAsync();
            }            
        }

        public async Task CalculateCompletion (int projectId)
        {
            var project = await GetProjectById(projectId);

            var todos = project.Todos;

            var completedTodos = project.Todos.Where(t => t.Status == "Completed");

            decimal percentage;

            if (completedTodos.Count() >= 1)
            {
                percentage = (completedTodos.Count() * 100) / todos.Count();

                percentage = Math.Round(percentage, 2);
            }
            else percentage = 0.00m;

            project.PercentageOfCompletion = percentage;

            _dataContext.Projects.Update(project);

            await _dataContext.SaveChangesAsync();
        }
    }
}
