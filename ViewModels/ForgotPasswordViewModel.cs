using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
