using Microsoft.AspNetCore.Mvc;
using ProjectPlanner.Data;
using ProjectPlanner.Models;
using ProjectPlanner.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Controllers
{

    public class TodosController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProjectRepository _projectRepository;

        public TodosController(ITodoRepository todoRepository, IProjectRepository projectRepository)
        {
            _todoRepository = todoRepository;
            _projectRepository = projectRepository;
        }

        // GET: Todos
        [Route("projects/{projectId:int}/todos")]
        public async Task<IActionResult> Index([FromRoute]int projectId, string searchString)
        {
            var todos = await _todoRepository.AllTodosByProjectId(projectId).ConfigureAwait(true);
            var project = await _todoRepository.GetProjectById(projectId).ConfigureAwait(true);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString.ToLower();

                todos = todos.Where(t => t.Name.ToLower().Contains(searchString)).ToList();
            }

            var model = new TodoListViewModel
            {
                Todos = todos,
                Project = project
            };

            return View(model);
        }

        //// GET: todos/create
        [HttpGet]
        [Route("projects/{projectId:int}/todos/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId:int}/todos/create")]
        public async Task<IActionResult> Create([Bind("Name,Description,EstimatedDate")] Todo todo, int projectId)
        {
            if (ModelState.IsValid)
            {
                await _todoRepository.CreateTodo(projectId, todo).ConfigureAwait(true);
                await _projectRepository.CalculateCompletion(projectId).ConfigureAwait(true);
            }
            return RedirectToRoute("todos", new { id = projectId });
        }

        //// GET: todos/todoId/edit
        [Route("projects/{projectId:int}/todos/{todoId:int}/edit")]
        public async Task<IActionResult> Edit(int projectId, int todoId)
        {
            if (todoId == 0)
            {
                return NotFound();
            }

            var todo = await _todoRepository.GetTodoById(todoId).ConfigureAwait(true);

            var model = new TodoViewModel
            {
                ProjectId = projectId,
                Todo = todo
            };

            return View(model);
        }

        //// POST: todos/todosId/edit
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId:int}/todos/{todoId:int}/edit")]

        public async Task<IActionResult> Edit(int projectId, int todoId, [Bind("Name, Description, EstimatedDate")] Todo todo)
        {
            if (todoId == 0 || projectId == 0 || todo == null)
            {
                return RedirectToAction("Index", "Projects");
            }

            if (ModelState.IsValid)
            {
                todo.TodoId = todoId;
                await _todoRepository.UpdateTodo(todo).ConfigureAwait(true);
            }

            return RedirectToRoute("todos", new { id = projectId });
        }

        //// GET: projects/id/delete
        [Route("projects/{projectId:int}/todos/{todoId:int}/delete")]
        public async Task<IActionResult> Delete(int todoId)
        {
            if (todoId == 0)
            {
                return NotFound();
            }

            var todo = await _todoRepository.GetTodoById(todoId).ConfigureAwait(true);

            if (todo == null)
            {
                return RedirectToRoute("todos", new { id = todo.Project.ProjectId });
            }


            return View(todo);
        }

        //// POST: projects/projectId/delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("projects/{projectId:int}/todos/{todoId:int}/delete")]
        public async Task<IActionResult> DeleteConfirmed(int projectId, int todoId)
        {
            await _todoRepository.DeleteTodo(todoId).ConfigureAwait(true);

            await _projectRepository.CalculateCompletion(projectId).ConfigureAwait(true);

            return RedirectToRoute("todos", new { id = projectId });
        }


        [Route("projects/{projectId:int}/todos/{todoId:int}/status")]
        public async Task<IActionResult> ChangeStatus(int todoId, int projectId, string status)
        {
            await _todoRepository.ChangeTodoStatus(todoId, status).ConfigureAwait(true);

            await _projectRepository.CalculateCompletion(projectId).ConfigureAwait(true);

            return RedirectToRoute("todos", new { id = projectId });
        }

    }
}
