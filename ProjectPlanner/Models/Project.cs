using ProjectPlanner.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPlanner.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "The name must be between 5 and 30 characters.")]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Description is required.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage ="The description must be between 10 and 200 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [FutureDate]
        [DisplayFormat(DataFormatString = "{0:dddd dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Estimated Date")]
        public DateTime EstimatedDate { get; set; }
        public List<Todo> Todos { get; set; } = new List<Todo>();

        public decimal  PercentageOfCompletion { get; set; }
    }
}
