using ProjectPlanner.Models;
using System.Collections.Generic;

namespace ProjectPlanner.ViewModels
{
    public class TodoListViewModel
    {
        public IEnumerable<Todo> Todos { get; set; }

        public Project Project { get; set; }
    }
}
