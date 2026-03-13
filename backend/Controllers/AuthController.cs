using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OnlinemedicineorderingdbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(OnlinemedicineorderingdbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
            {
                return BadRequest(new { Message = "Email already exists." });
            }

            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Hardcoded Admin
            if (dto.Email == "admin@admin.com" && dto.Password == "admin123")
            {
                var token = GenerateJwtToken(0, "Admin", "Admin");
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Role = "Admin",
                    UserId = 0,
                    Name = "Admin"
                });
            }

            // Customer Login (Using Phone as the 'Password' for simulation since there's no password column)
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == dto.Email && c.Phone == dto.Password);

            if (customer == null)
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            var customerToken = GenerateJwtToken(customer.Id, customer.Name, "Customer");

            return Ok(new AuthResponseDto
            {
                Token = customerToken,
                Role = "Customer",
                UserId = customer.Id,
                Name = customer.Name
            });
        }

        private string GenerateJwtToken(int userId, string name, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
