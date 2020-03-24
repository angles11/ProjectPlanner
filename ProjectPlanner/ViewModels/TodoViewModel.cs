using ProjectPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.ViewModels
{
    public class TodoViewModel
    {
        public Todo Todo { get; set; }

        public int ProjectId { get; set; }
    }
}
