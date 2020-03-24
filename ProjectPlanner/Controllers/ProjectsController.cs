using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Data;
using ProjectPlanner.Models;

namespace ProjectPlanner.Controllers
{
    [Route("projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        //// GET: Projects
        
        public IActionResult Index(string searchString)
        {
            var projects = _projectRepository.AllProjects.ToList();

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
                await _projectRepository.CreateProject(project);
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

            var project = await _projectRepository.GetProjectById(projectId);

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
            if (projectId == 0) return NotFound();

            if (ModelState.IsValid)
            {
                project.ProjectId = projectId;
                await _projectRepository.UpdateProject(project);
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

            var project = await _projectRepository.GetProjectById(projectId);

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
        public async Task<IActionResult> DeleteConfirmed(int projectId )
        {
            await _projectRepository.DeleteProject(projectId);

            return RedirectToAction(nameof(Index));
        }
        
        private async Task<bool> ProjectExists(int projectId)
        {
            return await _projectRepository.ProjectExists(projectId);
        }


    }
}
