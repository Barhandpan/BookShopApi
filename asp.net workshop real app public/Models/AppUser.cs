using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace asp.net_workshop_real_app_public.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public double Discounts { get; set; }
    }
}
