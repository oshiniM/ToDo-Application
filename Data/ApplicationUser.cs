using Microsoft.AspNetCore.Identity;    

namespace TodoApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
    }
}
