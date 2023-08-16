using asp.net_workshop_real_app_public.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using asp.net_workshop_real_app_public.Data;
using Microsoft.EntityFrameworkCore;

namespace asp.net_workshop_real_app_public.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly BookstoreContext _bookstoreContext;

        public AccountRepository(UserManager<AppUser> userManager, IConfiguration configuration, BookstoreContext bookstoreContext, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _bookstoreContext = bookstoreContext;
        }

        public async Task<IdentityResult> SignUpAdmin(SignupModel signupModel)
        {
            AppUser user = new()
            {
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Email = signupModel.Email,
                UserName = signupModel.Email,
                Discounts = 0,
            };
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync("Admin");

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                await _userManager.AddToRoleAsync(user, "Admin");

            }

            return result;
        }
        public async Task<IdentityResult> SignUp(SignupModel signupModel)
        {
            AppUser user = new()
            {
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Email = signupModel.Email,
                UserName = signupModel.Email,
                Discounts = 0,
            };
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }
        /* public async Task<IdentityResult> SignUp(SignupModel signupModel)
         {

             AppUser user = new AppUser
             {

                 Email = signupModel.Email,
                 UserName = signupModel.Email,

             };

             var result = await _userManager.CreateAsync(user, signupModel.Password);

             if (result.Succeeded)
             {
                 await _userManager.AddToRoleAsync(user, "User");
             }

             return result;
         }*/

        public async Task<Object> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!result)
            {
                return null;
            }
            var token = await GenerateToken(user);
            return token;
        }
        public async Task<AppUser> GetUserDetails(string userEmail)
        {
            var user = await _bookstoreContext.Users.FirstOrDefaultAsync(user => user.Email == userEmail);
            Console.WriteLine(userEmail);
            return user;
        }
        public async Task<double> GetDiscount(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            return user?.Discounts ?? 0.0;
        }
        public async Task UpdateDiscount(string userEmail, double discount)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                user.Discounts = discount;
                await _userManager.UpdateAsync(user);
            }
        }
        public async Task UpdateAccount(string userEmail, string firstName, string lastName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {

                user.FirstName = firstName;
                user.LastName = lastName;
                await _userManager.UpdateAsync(user);
            }
        }
        public async Task<IdentityResult> DeleteUser(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result;
            }

            return null;
        }
        public async Task<bool> IsAdmin(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains("Admin");
        }

        private async Task<string> GenerateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
