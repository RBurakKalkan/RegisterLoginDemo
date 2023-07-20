using Microsoft.EntityFrameworkCore;
using RegisterLoginDemo.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace RegisterLoginDemo.Application.Data
{
 public class LoginDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public LoginDbContext(DbContextOptions<LoginDbContext> options)
            : base(options)
        {
        }
    }
}
