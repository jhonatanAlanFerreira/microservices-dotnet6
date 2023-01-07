using System.ComponentModel.DataAnnotations;

namespace GeekShopping.IdentityServer.Pages.Account.Register
{
    public class RegisterInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string RoleName { get; set; }
    }
}
