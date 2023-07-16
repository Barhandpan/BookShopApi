using System.ComponentModel.DataAnnotations;

namespace asp.net_workshop_real_app_public.Models
{
    public class SignupModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
