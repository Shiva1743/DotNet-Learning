using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        [Compare("NewPassword",ErrorMessage = "confirm password not matched within new password.")]
        public string ConfirmPassword { get; set; }
    }
}
