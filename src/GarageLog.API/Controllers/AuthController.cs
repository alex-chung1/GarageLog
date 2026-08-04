using GarageLog.API.Extensions;
using GarageLog.Application.DTOs.Auth;
using GarageLog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageLog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IWebHostEnvironment environment)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);

        SetAuthCookie(response.Token);

        return Ok(
            new UserResponse
            {
                UserId = response.UserId,
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName,
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);

        SetAuthCookie(response.Token);

        return Ok(
            new UserResponse
            {
                UserId = response.UserId,
                Email = response.Email,
                FirstName = response.FirstName,
                LastName = response.LastName,
            }
        );
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var response = new UserResponse
        {
            UserId = this.GetUserId(),
            Email = this.GetUserEmail(),
            FirstName = this.GetUserFirstName(),
            LastName = this.GetUserLastName(),
        };

        return Ok(response);
    }

    private void SetAuthCookie(string token)
    {
        bool isDevelopment = environment.IsDevelopment();

        Response.Cookies.Append(
            "auth_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !isDevelopment,
                SameSite = !isDevelopment ? SameSiteMode.Strict : SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7),
            }
        );
    }
}
