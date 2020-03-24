using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public interface IProjectRepository
    {
        public IEnumerable<Project> AllProjects { get; }

        public Task<bool> ProjectExists(int projectId);
        public  Task<Project> GetProjectById(int projectId);
   
        public Task CreateProject(Project project);

        public Task UpdateProject(Project project);

        public Task DeleteProject(int projectId);

        public Task CalculateCompletion(int projectId);
    }
}
