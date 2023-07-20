using RegisterLoginDemo.Application.Abstraction.Service;
using RegisterLoginDemo.Domain.Abstraction.Repository;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Persistance.Concrete
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly IVerificationCodeRepository _verificationCodeRepository;

        public VerificationCodeService(IVerificationCodeRepository verificationCodeRepository)
        {
            _verificationCodeRepository = verificationCodeRepository;
        }

        public string GenerateVerificationCode(int codeLength)
        {
            const string allowedChars = "0123456789";
            var random = new Random();
            var code = new string(Enumerable.Repeat(allowedChars, codeLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return code;
        }

        public void StoreVerificationCode(int userId, string code, double addMinutes, SendType type)
        {
            var verificationCode = new VerificationCode
            {
                Code = code,
                UserId = userId,
                ExpirationTime = DateTime.UtcNow.AddMinutes(addMinutes),
                Type = type,
                IsVerified = false
            };

            _verificationCodeRepository.Add(verificationCode);
        }

        public async Task<bool> VerifyCode(int userId, string code)
        {
            var verificationCode = await _verificationCodeRepository.GetUnverifiedCode(userId, code);

            if (verificationCode != null && verificationCode.ExpirationTime >= DateTime.UtcNow)
            {
                verificationCode.IsVerified = true;
                _verificationCodeRepository.Update(verificationCode);
                return true;
            }
            return false;
        }
    }
}
