using System.ComponentModel.DataAnnotations;

namespace ProjectPlanner.ViewModels
{
    public class EditPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

    }
}
