using GarageLog.Application.DTOs.Auth;
using GarageLog.Application.Interfaces.Services;
using GarageLog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace GarageLog.Infrastructure.Services;

public class AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ArgumentException(
                string.Join(", ", result.Errors.Select(e => e.Description))
            );

        string token = tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName
        );

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        bool isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Invalid credentials");

        string token = tokenService.GenerateToken(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName
        );

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
    }
}
