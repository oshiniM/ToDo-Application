using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Entities;
using Microsoft.AspNetCore.Authorization;
using TodoApp.Services;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {

        private readonly IUserAuthService _authService;
        private object _signInManager;

        public UserAuthController(IUserAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var result = await _authService.RegisterAsync(registerModel);
            return result == null ? Ok("User Created Successfully") : BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var token = await _authService.LoginAsync(loginModel);
            return token != null ? Ok(new { success = true, token }) : Unauthorized(new { success = false, message = "Invalid Username or Password" });
        }

        [HttpPost("Logout")]
        [Authorize] 
        public async Task<IActionResult> Logout([FromBody]LoginModel loginModel)
        {
            var token = await _authService.LoginAsync(loginModel);
            return Ok("User Logout Successfully");
        }
    }
}
