using ELeaveAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokens;

    public AuthController(AppDbContext db, TokenService tokens)
    { _db = db; _tokens = tokens; }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u =>
                u.Username == dto.Username && u.IsActive);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(
                new { message = "Invalid username or password" });

        return Ok(new LoginResponseDto
        {
            Token = _tokens.GenerateToken(user),
            UserID = user.UserID,
            FullName = user.FullName,
            Role = user.Role,
            Department = user.Department ?? string.Empty
        });
    }

    // POST api/auth/hash  — use this once to generate BCrypt hashes
    [HttpPost("hash")]
    public IActionResult HashPassword([FromBody] string plainText)
        => Ok(BCrypt.Net.BCrypt.HashPassword(plainText));

    // POST: /auth/signup
    [HttpPost("signup")]
    public IActionResult Signup([FromBody] SignupDto dto)
    {
        // Check if username exists
        var existingUser = _db.Users.FirstOrDefault(u => u.Username == dto.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Username already exists." });
        }

        // Hash the password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // Create user
        var user = new User
        {
            FullName = dto.FullName,
            Username = dto.Username,
            PasswordHash = passwordHash,
            Role = "Employee" // default role
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(new { message = "Signup successful!" });
    }
}
