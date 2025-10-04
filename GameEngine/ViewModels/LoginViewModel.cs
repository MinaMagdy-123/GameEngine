using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameEngine.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
