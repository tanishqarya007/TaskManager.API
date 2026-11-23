
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Api.Data;
using TaskManager.Api.Dtos;
using TaskManager.Api.Models;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly AppDbContext _db;
	private readonly TokenService _tokens;

	public AuthController(AppDbContext db, TokenService tokens)
	{
		_db = db;
		_tokens = tokens;
	}

	[HttpPost("register")]
	public async Task<ActionResult<AuthResponse>> Register(RegisterDto dto)
	{
		var email = dto.Email.Trim().ToLowerInvariant();
		if (await _db.Users.AnyAsync(u => u.Email == email))
			return Conflict(new { message = "Email already registered." });

		var user = new User
		{
			Email = email,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
		};
		_db.Users.Add(user);
		await _db.SaveChangesAsync();
		return Ok(new AuthResponse(_tokens.CreateToken(user)));
	}

	[HttpPost("login")]
	public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
	{
		var email = dto.Email.Trim().ToLowerInvariant();
		var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
		if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
			return Unauthorized(new { message = "Invalid credentials." });

		return Ok(new AuthResponse(_tokens.CreateToken(user)));
	}
}