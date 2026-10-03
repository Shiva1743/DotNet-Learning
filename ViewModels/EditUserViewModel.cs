using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }
        //public string Password { get; set; }

        [Required][EmailAddress]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }

        public IList<string> Claims {  get; set; } = new List<string>();
        public IList<string> Roles {  get; set; } = new List<string>();

    }
}
