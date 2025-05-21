using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Data.AppDbContext _context;
        private readonly string _secretKey = "your-256-bit-secret"; // Güvenlik için bunu gizli tutmalısınız

        public AuthController(Data.AppDbContext context)
        {
            _context = context;
        }

        // Giriş kontrolü yapacak endpoint
        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _context.users
                               .FirstOrDefault(u => u.username == loginUser.username);

            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            // Bcrypt ile şifreyi doğrula
            if (!BCrypt.Net.BCrypt.Verify(loginUser.password_hash, user.password_hash))
            {
                return Unauthorized("Yanlış şifre.");
            }

            // JWT Token oluştur
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Role, user.role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Token'ı döndür
            return Ok(new { token = tokenString, role = user.role });
        }
    }
}
