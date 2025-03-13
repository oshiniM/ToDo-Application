using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Entities;

namespace TodoApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 
        
        }

        public required DbSet<TaskItem> Tasks { get; set; }
    }
}
