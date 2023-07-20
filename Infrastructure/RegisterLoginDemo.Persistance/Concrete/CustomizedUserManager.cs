using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegisterLoginDemo.Application.Data;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Persistance.Concrete
{
    public class CustomizedUserManager<TUser> : UserManager<TUser> where TUser : class
    {
        private readonly LoginDbContext _dbContext;
        public CustomizedUserManager(
            LoginDbContext dbContext,
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext =
                dbContext;
        }

        public async Task<User?> FindByPhoneAsync(string phoneNumber)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<User?> FindByEmailAsync(string Email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }
    }
}
