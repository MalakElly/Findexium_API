using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Services; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration _config;

        public LoginController(UserRepository userRepository, IPasswordHasher hasher, IConfiguration config)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifie si l’utilisateur existe
            var user = await _userRepository.FindByUserNameAsync(model.Username);
            if (user == null)
                return Unauthorized("Utilisateur introuvable");

            // Vérifie le mot de passe (hash vs mot de passe saisi)
            if (!_hasher.Verify(user.PasswordHash, model.Password))
                return Unauthorized("Mot de passe invalide");

            // Génération du JWT
            var jwt = _config.GetSection("Jwt");
            var x = jwt["key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "USER")
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8), // durée de vie du token
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new { user.Id, user.Username, user.Role }
            });
        }
    }
}
