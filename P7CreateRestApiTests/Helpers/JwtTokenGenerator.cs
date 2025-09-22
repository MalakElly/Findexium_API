using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P7CreateRestApiTests.Helpers
{
        public static class JwtTokenGenerator
        {
            public static string GenerateToken(string role, string key, string issuer, string audience)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim("uid", "1"),
                new Claim(ClaimTypes.Role, role)
            };

                var creds = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    
}
