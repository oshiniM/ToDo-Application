using TodoApp.Data;
using TodoApp.Entities;

namespace TodoApp.Services
{
    public interface IUserAuthService
    {
        Task<string?> RegisterAsync(RegisterModel registerModel);
        Task<string?> LoginAsync(LoginModel loginModel);
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
