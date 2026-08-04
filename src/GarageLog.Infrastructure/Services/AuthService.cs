using GarageLog.Application.DTOs.Auth;
using GarageLog.Application.Exceptions;
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
        {
            var errors = result
                .Errors.Select(e =>
                    e.Code switch
                    {
                        "DuplicateEmail" => "An account with this email already exists.",

                        "DuplicateUserName" => null,

                        _ => e.Description,
                    }
                )
                .Where(e => e != null)
                .Cast<string>()
                .ToList();

            throw new ValidationException(errors);
        }

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
            throw new UnauthorizedAccessException("Invalid email or password");

        bool isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Invalid email or password");

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
