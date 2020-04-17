using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanner.Models
{
    public class User : IdentityUser
    {

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Job Position")]
        public string Position { get; set; }

        [Display(Name = "Experience")]
        public string Experience { get; set; }

        [Display(Name = "Company")]
        public string Company { get; set; }

        public bool IsExternal { get; set; } = false;

        public List<Project> Projects { get; } = new List<Project>();

    }
}
