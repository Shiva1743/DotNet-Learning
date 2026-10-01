using System.ComponentModel.DataAnnotations;

namespace CoreEmptyProject1.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
