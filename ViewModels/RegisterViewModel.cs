using CoreEmptyProject1.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [ValidEmailDomain(allowedDomain: "test.com", ErrorMessage = "domain name must be test.com")]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }




        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name= "Confirm Password")]
        [Compare("Password",ErrorMessage="Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
