using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.Models
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Length not exceed 10 character")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Office Email")] // Label change karva html page ma
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        public IFormFile Photo { get; set; }
    }
}
