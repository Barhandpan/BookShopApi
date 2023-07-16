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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel signinModel)
        {
            var res = await _accountRepository.Login(signinModel);
            if (res == null)
            {
                return Unauthorized();
            }
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
        [HttpGet("Discount")]
        public async Task<IActionResult> GetDiscount([FromHeader] string userEmail)
        {
/*            var userEmail = User.Identity.Name;
*/            var discount = await _accountRepository.GetDiscount(userEmail);

            return Ok(discount);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Discount")]
        public async Task<IActionResult> AddDiscount(double discount)
        {
            var userEmail = User.Identity.Name;
            await _accountRepository.AddDiscount(userEmail, discount);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Discount")]
        public async Task<IActionResult> UpdateDiscount(double discount)
        {
            var userEmail = User.Identity.Name;
            await _accountRepository.UpdateDiscount(userEmail, discount);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Discount")]
        public async Task<IActionResult> DeleteDiscount()
        {
            var userEmail = User.Identity.Name;
            await _accountRepository.DeleteDiscount(userEmail);
            return Ok();
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateAccount([FromHeader] string userEmail, [FromBody] UpdatedAccountModel model)
        {
            await _accountRepository.UpdateAccount(userEmail, model.FirstName, model.LastName);
            var userDetails = await _accountRepository.GetUserDetails(userEmail);

            return Ok(userDetails);
        }
        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUser([FromHeader] string userEmail)
        {
            var result = await _accountRepository.DeleteUser(userEmail);
            if (result != null)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
