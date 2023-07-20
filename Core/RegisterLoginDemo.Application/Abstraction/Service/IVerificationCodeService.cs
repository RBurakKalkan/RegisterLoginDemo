
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Application.Abstraction.Service
{
    public interface IVerificationCodeService
    {
        string GenerateVerificationCode(int codeLength);
        void StoreVerificationCode(int userId, string code, double addMinutes, SendType type);
        Task<bool> VerifyCode(int userId, string code);
    }
}