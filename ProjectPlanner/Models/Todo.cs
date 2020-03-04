using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MyProperty { get; set; }
        public DateTime DateOfCreation { get; set; }

        public DateTime EstimatedDate { get; set; }
        public string Status { get; set; }
    }
}
