using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public UserController(UserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        // POST api/user (register)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.FindByUserNameAsync(dto.Username);
            if (existingUser != null)
                return Conflict("Ce nom d'utilisateur est déjà pris.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = _hasher.Hash(dto.Password),
                Fullname = dto.Fullname,
                Role = string.IsNullOrEmpty(dto.Role) ? "USER" : dto.Role
            };

            await _userRepository.AddAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Fullname = user.Fullname,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, userDto);
        }

        // GET api/user
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.FindAllAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Fullname = u.Fullname,
                Role = u.Role
            });

            return Ok(userDtos);
        }

        // GET api/user/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Fullname = user.Fullname,
                Role = user.Role
            };

            return Ok(userDto);
        }

        // PUT api/user/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _userRepository.FindByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Username = dto.Username;
            existing.Fullname = dto.Fullname;
            existing.Role = dto.Role;

            await _userRepository.UpdateAsync(existing);

            return NoContent();
        }

        // DELETE api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteAsync(user);
            return NoContent();
        }
    }
}
