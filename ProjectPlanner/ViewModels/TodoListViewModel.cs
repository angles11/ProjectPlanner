using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.ViewModels
{
    public class TodoListViewModel
    {
        public IEnumerable<Todo> Todos { get; set; }

        public Project Project { get; set; }
    }
}
