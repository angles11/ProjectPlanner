using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectPlanner.Data;
using ProjectPlanner.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Controllers
{
    [Route("projects")]
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<User> _userManager;

        public ProjectsController(IProjectRepository projectRepository, UserManager<User> userManager)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        //// GET: Projects

        public async Task<IActionResult> Index(string searchString)
        {
            //var projects = _projectRepository.AllProjects.ToList();

            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(true);

            if (user == null)
            {
                return NotFound();
            }

            var projects = _projectRepository.GetAllProjects(user).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString.ToLower();

                projects = projects.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
            }
            return View(projects);
        }

        //// GET: projects/create
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        //// POST: projects/create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        ///
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Name,Description,EstimatedDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(true);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    await _projectRepository.CreateProject(project, user).ConfigureAwait(true);
                }
                catch (ArgumentNullException)
                {

                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //// GET: projects/id/edit
        [Route("projectId:int/edit")]
        public async Task<IActionResult> Edit(int projectId)
        {
            if (projectId == 0)
            {
                return NotFound();
            }

            var project = await _projectRepository.GetProjectById(projectId).ConfigureAwait(true);

            return View(project);
        }

        //// POST: projects/projectId/edit
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projectId:int/edit")]

        public async Task<IActionResult> Edit(int projectId, [Bind("Name, Description, EstimatedDate")] Project project)
        {
            if (project == null || projectId == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                project.ProjectId = projectId;
                await _projectRepository.UpdateProject(project).ConfigureAwait(true);
            }

            return RedirectToAction(nameof(Index));
        }

        //// GET: projects/id/delete
        [Route("projectId:int/delete")]
        public async Task<IActionResult> Delete(int projectId)
        {
            if (projectId == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var project = await _projectRepository.GetProjectById(projectId).ConfigureAwait(true);

            if (project == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        //// POST: projects/projectId/delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("projectId:int/delete")]
        public async Task<IActionResult> DeleteConfirmed(int projectId)
        {
            await _projectRepository.DeleteProject(projectId).ConfigureAwait(true);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProjectExists(int projectId)
        {
            return await _projectRepository.ProjectExists(projectId).ConfigureAwait(true);
        }


    }
}
