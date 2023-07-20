using Microsoft.AspNetCore.Identity;
using RegisterLoginDemo.Application.Abstraction.Service;
using RegisterLoginDemo.Application.ViewModel;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Persistance.Concrete
{
    public class UserService : IUserService
    {
        private readonly CustomizedUserManager<User> _userManager;

        public UserService(CustomizedUserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(User model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists." });
            }

            existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email already exists." });
            }

            existingUser = await _userManager.FindByPhoneAsync(model.PhoneNumber);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Phone number already exists." });
            }

            var user = new User { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };

            var result = await _userManager.CreateAsync(user, model.PasswordHash);
            return result;
        }
        public async Task<User?> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return null;
            }

            var password = await _userManager.CheckPasswordAsync(user, model.Password);

            return password ? user : null;
        }
    }
}
