using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime EstimatedDate { get; set; }
        public List<Todo> Todos { get; set; }

        public string Status { get; set; }
    }
}
