using ProjectPlanner.Models;
using System.Collections.Generic;

namespace ProjectPlanner.ViewModels
{
    public class EditAccountViewModel
    {
        public User User { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Todo> Todos { get; set; }

    }
}
