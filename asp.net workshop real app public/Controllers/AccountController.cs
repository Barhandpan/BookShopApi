using asp.net_workshop_real_app_public.Models;
using asp.net_workshop_real_app_public.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace asp.net_workshop_real_app_public.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Signup([FromBody] SignupModel signupModel)
        {
            var res = await _accountRepository.SignUp(signupModel);
            if (res.Succeeded)
            {
                return Ok(res.Succeeded);
            }
            return Unauthorized();
        }
        [HttpPost("Admin")]
        public async Task<IActionResult> SignupAdmin([FromBody] SignupModel signupModel)
        {
            var res = await _accountRepository.SignUpAdmin(signupModel);
            if (res.Succeeded)
            {
                return Ok(res);
            }
            return Unauthorized();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel signinModel)
        {
            var res = await _accountRepository.Login(signinModel);
            if (res == null)
            {
                return Unauthorized();
            }
            Console.WriteLine(res);
            return Ok(res);
        }
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> UserDetails()
        {
            var userEmail = User.Identity.Name;
            Console.WriteLine(userEmail);
            var userDetails = await _accountRepository.GetUserDetails(userEmail);
            return Ok(userDetails);
        }
        [Authorize]
        [HttpGet("Discount")]
        public async Task<IActionResult> GetDiscount()
        {
            var userEmail = User.Identity.Name;

            var discount = await _accountRepository.GetDiscount(userEmail);

            return Ok(discount);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Discount")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountModel userDetails)
        {
            var userEmail = User.Identity.Name;

            if (!User.IsInRole("Admin"))
            {
                return Forbid(); // Return 403 Forbidden if the user is not an admin
            }

            await _accountRepository.UpdateDiscount(userDetails.userEmail, userDetails.Discounts);
            return Ok();
        }

        [Authorize]
        [HttpPut("user")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdatedAccountModel model)
        {
            var userEmail = User.Identity.Name;
            await _accountRepository.UpdateAccount(userEmail, model.FirstName, model.LastName);
            var userDetails = await _accountRepository.GetUserDetails(userEmail);

            return Ok(userDetails);
        }
        [Authorize]
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUser()
        {
            var userEmail = User.Identity.Name;
            var result = await _accountRepository.DeleteUser(userEmail);
            if (result != null)
            {
                return Ok();
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet("Admin")]
        public async Task<IActionResult> IsAdmin()
        {
            var userEmail = User.Identity.Name;
            var isAdmin = await _accountRepository.IsAdmin(userEmail);

            return Ok(isAdmin);
        }
    }
}
