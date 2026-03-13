using System;

namespace backend.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!; // Using Phone or hardcoded secret
    }

    public class RegisterDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
    }
}
