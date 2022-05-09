using System.ComponentModel.DataAnnotations;

namespace GroceryStoresApp.ViewModels
{
    public class CreateRoleViewModels
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
