using Microsoft.AspNetCore.Identity;
using RegisterLoginDemo.Application.ViewModel;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Application.Abstraction.Service
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterAsync(User model);
        public Task<User?> LoginAsync(LoginRequest model);
    }
}
