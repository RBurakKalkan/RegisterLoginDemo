using Microsoft.EntityFrameworkCore;
using RegisterLoginDemo.Application.Data;
using RegisterLoginDemo.Domain.Abstraction.Repository;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Application.Concrete.Repository
{
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly LoginDbContext _dbContext;

        public VerificationCodeRepository(LoginDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(VerificationCode verificationCode)
        {
            _dbContext.VerificationCodes.Add(verificationCode);
            _dbContext.SaveChanges();
        }

        public void Update(VerificationCode verificationCode)
        {
            _dbContext.VerificationCodes.Update(verificationCode);
            _dbContext.SaveChanges();
        }

        public async Task<VerificationCode?> GetUnverifiedCode(int userId, string code)
        {
            return await _dbContext.VerificationCodes
                .FirstOrDefaultAsync(vc => vc.UserId == userId && !vc.IsVerified && vc.Code == code);
        }
    }
}
