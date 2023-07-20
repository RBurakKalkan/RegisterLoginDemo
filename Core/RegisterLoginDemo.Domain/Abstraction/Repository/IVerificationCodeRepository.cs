using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Domain.Abstraction.Repository
{
    public interface IVerificationCodeRepository
    {
        void Add(VerificationCode verificationCode);
        void Update(VerificationCode verificationCode);
        Task<VerificationCode?> GetUnverifiedCode(int userId, string code);
    }
}
