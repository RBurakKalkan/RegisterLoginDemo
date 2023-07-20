
namespace RegisterLoginDemo.Application.Abstraction.Service
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(string verificationCode);
    }
}
