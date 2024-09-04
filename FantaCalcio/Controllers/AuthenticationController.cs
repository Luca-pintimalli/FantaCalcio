using System;
using FantaCalcio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FantaCalcio.Data;
using FantaCalcio.DTOs;
using FantaCalcio.Helpers;

namespace FantaCalcio.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly byte[] _key;

        public AuthenticationController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (_context.Utenti.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email already in use.");
            }

            var hashedPassword = PasswordHelper.HashPassword(model.Password);
            var utente = new Utente
            {
                Nome = model.Nome,
                Cognome=model.Cognome,
                Email = model.Email,
                Password = hashedPassword,
                Foto = model.Foto
            };

            _context.Utenti.Add(utente);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Utenti.SingleOrDefault(u => u.Email == model.Email);
            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.Password))
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Nome),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(_key);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1); // Scadenza del token impostata a 1 anno
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return Ok(new LoginResponseModel
            {
                UserName = user.Nome,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expiration
            });
        }
    }
}