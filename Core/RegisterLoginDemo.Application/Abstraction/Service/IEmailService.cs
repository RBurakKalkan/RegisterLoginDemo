
namespace RegisterLoginDemo.Application.Abstraction.Service
{
    public interface IEmailService
    {
        public void SendVerificationCode(string recipient, string message, string subject);
    }
}
