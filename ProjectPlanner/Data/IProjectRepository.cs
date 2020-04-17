using ProjectPlanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectPlanner.Data
{
    public interface IProjectRepository
    {
        public IEnumerable<Project> GetAllProjects(User user);

        public Task<bool> ProjectExists(int projectId);
        public Task<Project> GetProjectById(int projectId);

        public Task CreateProject(Project project, User user);

        public Task UpdateProject(Project project);

        public Task DeleteProject(int projectId);

        public Task CalculateCompletion(int projectId);
    }
}
