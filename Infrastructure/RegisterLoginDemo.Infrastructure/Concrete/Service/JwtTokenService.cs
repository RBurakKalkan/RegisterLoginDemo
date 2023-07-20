using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RegisterLoginDemo.Application.Abstraction.Service;

namespace RegisterLoginDemo.Infrastructure.Concrete.Service
{
    public class JwtTokenService : IJwtTokenService
    {
        string secretKeyBase;
        public JwtTokenService(IConfiguration _config)
        {
            secretKeyBase = _config.GetValue<string>("SmsServiceProvider:SecretKeyBase");
        }
        public string GenerateJwtToken(string verificationCode)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var combinedKey = $"{secretKeyBase}-{verificationCode}";
            string jwtToken = string.Empty;
            using (var hmac = new HMACSHA256())
            {
                var keyBytes = hmac.Key; // Key size is automatically set to the required size
                var key = Encoding.ASCII.GetBytes(combinedKey)
                    .Concat(keyBytes)
                    .ToArray();

                // Create the JWT token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                jwtToken = tokenHandler.WriteToken(token);
            }
            return jwtToken;
        }
    }
}
