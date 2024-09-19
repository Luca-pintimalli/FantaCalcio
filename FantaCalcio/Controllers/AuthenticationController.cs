using FantaCalcio.Data;
using FantaCalcio.DTOs;
using FantaCalcio.Helpers;
using FantaCalcio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
    public async Task<IActionResult> Register([FromForm] RegisterModel model, IFormFile? foto)
    {
        try
        {
            // Controlla se l'email è già in uso
            if (_context.Utenti.Any(u => u.Email == model.Email))
            {
                // Restituisci un errore 400 con un messaggio chiaro
                return BadRequest(new { message = "Email già in uso." });
            }
            // Hash della password
            var hashedPassword = PasswordHelper.HashPassword(model.Password);

            // Variabile per il percorso della foto
            string? fotoPath = null;

            // Gestisci l'upload della foto solo se esiste
            if (foto != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(fileStream);
                }

                fotoPath = $"/uploads/{uniqueFileName}";
            }
            else
            {
                // Se nessuna foto viene caricata, usa l'immagine di default
                fotoPath = "/uploads/default-avatar.jpg";
            }

            // Crea l'entità Utente, includendo la foto (o quella di default)
            var utente = new Utente
            {
                Nome = model.Nome,
                Cognome = model.Cognome,
                Email = model.Email,
                Password = hashedPassword,
                Foto = fotoPath, // Usa il percorso della foto caricata o di default
                DataRegistrazione = DateTime.UtcNow
            };

            _context.Utenti.Add(utente);
            await _context.SaveChangesAsync();


            return Ok(new { Message = "User registered successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore interno del server: {ex.Message}");
        }
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
    new Claim(ClaimTypes.NameIdentifier, user.ID_Utente.ToString()),  // Assicurati che sia numerico
    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};



        var key = new SymmetricSecurityKey(_key);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddYears(1);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return Ok(new LoginResponseModel
        {
            id = user.ID_Utente,
            UserName = user.Nome,
            Cognome = user.Cognome,
            Email = user.Email,
            Foto = user.Foto,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = expiration,
            DataRegistrazione = user.DataRegistrazione
        });
    }


    [HttpPut("update-profile-picture/{id}")]
    public async Task<IActionResult> UpdateProfilePicture(int id, IFormFile foto)
    {
        var utente = await _context.Utenti.FindAsync(id);
        if (utente == null)
        {
            return NotFound();
        }

        if (foto != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            utente.Foto = $"/uploads/{uniqueFileName}";
            _context.Utenti.Update(utente);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Immagine aggiornata con successo.", Foto = utente.Foto });
        }
        return BadRequest("Nessun file fornito.");
    }




}

