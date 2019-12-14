using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Common;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ducode.Wolk.Identity.Impl
{
    internal class JwtManager : IJwtManager
    {
        private readonly IDateTime _dateTime;
        private readonly IdentityConfiguration _config;

        public JwtManager(
            IDateTime dateTime,
            IOptions<IdentityConfiguration> options)
        {
            _dateTime = dateTime;
            _config = options.Value;
        }

        public string CreateJwt(User user)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_config.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(CustomClaimTypes.SecurityStamp, user.SecurityStamp)
                }),
                Expires = _dateTime.Now.AddSeconds(_config.ExpirationInSeconds).DateTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
