using asp.net_workshop_real_app_public.Models;
using Microsoft.AspNetCore.Identity;

namespace asp.net_workshop_real_app_public.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAdmin(SignupModel signupModel);
        Task<IdentityResult> SignUp(SignupModel signupModel);
        Task<Object> Login(LoginModel loginModel);

        Task<double> GetDiscount(string userEmail);

        Task UpdateDiscount(string userEmail, double discount);
        Task<AppUser> GetUserDetails(string userEmail);
        Task UpdateAccount(string userEmail, string firstName, string lastName);
        Task<IdentityResult> DeleteUser(string userEmail);
        Task<bool> IsAdmin(string userEmail);
    }
}