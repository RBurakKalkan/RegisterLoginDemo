using Microsoft.AspNetCore.Mvc;
using RegisterLoginDemo.Application.Abstraction.Service;
using RegisterLoginDemo.Application.ViewModel;
using RegisterLoginDemo.Domain.Entities;

namespace RegisterLoginDemo.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ApiController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IJwtTokenService _jwtToken;
        private readonly IUserService _userService;

        public ApiController(
            IEmailService emailService,
            ISmsService smsService,
            IVerificationCodeService verificationCodeService,
            IJwtTokenService jwtTokenService,
            IUserService userService)
        {
            _verificationCodeService = verificationCodeService;
            _emailService = emailService;
            _smsService = smsService;
            _jwtToken = jwtTokenService;
            _userService = userService;
        }

        [HttpPost("SendVerificationCode")]
        public IActionResult SendVerificationCode([FromBody] SendVerificationCodeRequest request)
        {
            var verificationCode = _verificationCodeService.GenerateVerificationCode(6);
            _verificationCodeService.StoreVerificationCode(request.User.Id, verificationCode, 3, request.DeliveryMethod);
            if (request.DeliveryMethod == SendType.SMS)
            {
                _smsService.SendVerificationCode(request.User.PhoneNumber, $"Your verification code is : {verificationCode}");
                return Ok("Verification code sent.");
            }
            else if (request.DeliveryMethod == SendType.Email)
            {
                _emailService.SendVerificationCode(request.User.Email, $"Your verification code is : {verificationCode}", "Verification Code");
                return Ok("Verification code sent.");
            }
            else
            {
                return BadRequest("Invalid delivery method");
            }
        }
        [HttpPost("VerifyCodeAndSignIn")]
        public async Task<IActionResult> VerifyCodeAndSignIn([FromBody] VerifyCodeAndSignInRequest request)
        {
            var storedVerificationCode = await _verificationCodeService.VerifyCode(request.User.Id, request.verificationCode);

            if (storedVerificationCode)
            {
                return Ok(new { Token = _jwtToken.GenerateJwtToken(request.verificationCode) });
            }
            else
            {
                return BadRequest("Verification code not found.");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userService.LoginAsync(model);
            return user != null ? Ok(user) : BadRequest("Invalid username or password.");
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] User model)
        {
            var result = await _userService.RegisterAsync(model);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
