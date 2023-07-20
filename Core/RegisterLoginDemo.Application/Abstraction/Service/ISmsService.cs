
namespace RegisterLoginDemo.Application.Abstraction.Service
{
    public interface ISmsService
    {
        public void SendVerificationCode(string recipient, string message);
    }
}
