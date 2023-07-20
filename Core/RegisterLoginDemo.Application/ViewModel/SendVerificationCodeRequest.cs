using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.Application.ViewModel
{
    public class SendVerificationCodeRequest
    {
        public User? User { get; set; }
        public SendType DeliveryMethod { get; set; }
    }
}
