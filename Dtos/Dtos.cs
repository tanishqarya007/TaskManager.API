using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Dtos;

public record RegisterDto([Required, EmailAddress] string Email, [Required, MinLength(6)] string Password);
public record LoginDto([Required] string Email, [Required] string Password);
public record AuthResponse(string Token);
public record TaskCreateDto([Required, MaxLength(200)] string Title, string? Description);
public record TaskUpdateDto([Required, MaxLength(200)] string Title, string? Description, bool IsCompleted);