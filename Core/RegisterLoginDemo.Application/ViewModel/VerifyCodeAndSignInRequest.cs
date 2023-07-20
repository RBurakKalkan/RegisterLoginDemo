using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Application.ViewModel
{
    public class VerifyCodeAndSignInRequest
    {
        public User? User { get; set; }
        public string? verificationCode { get; set; }
    }
}
