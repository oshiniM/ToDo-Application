using Microsoft.AspNetCore.Identity;
using TodoApp.Data;
using TodoApp.Entities;
using TodoApp.Utilities;

namespace TodoApp.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UserAuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            // Get JWT settings from configuration
            var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
            var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing");
            var jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing");
            var jwtExpiry = int.TryParse(configuration["Jwt:ExpiryMinutes"], out int expiry) ? expiry : 60;

            // Instantiate JwtTokenGenerator with configuration values
            _jwtTokenGenerator = new JwtTokenGenerator(jwtKey, jwtIssuer, jwtAudience, jwtExpiry);
        }

        public async Task<string?> RegisterAsync(RegisterModel registerModel)
        {
            if (registerModel == null
                || string.IsNullOrEmpty(registerModel.Name)
                || string.IsNullOrEmpty(registerModel.Email)
                || string.IsNullOrEmpty(registerModel.Password))
            {
                return "Invalid registration details";
            }

            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                return "Email already exists";
            }

            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                Name = registerModel.Name
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            return result.Succeeded ? null : "Error creating user";
        }

        public async Task<string?> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null || !(await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false)).Succeeded)
            {
                return null;
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public Task<string> GenerateJwtToken(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
