using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace GarageLog.API.Extensions;

public static class ControllerBaseExtensions
{
    public static int GetUserId(this ControllerBase controller)
    {
        string? userIdClaim = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("Missing or invalid user identifier claim.");
        }

        return userId;
    }

    public static string GetUserEmail(this ControllerBase controller)
    {
        string? email = controller.User.FindFirstValue(ClaimTypes.Email);

        if (email is null)
        {
            throw new UnauthorizedAccessException("Missing email claim.");
        }

        return email;
    }

    public static string GetUserFirstName(this ControllerBase controller)
    {
        string? firstName = controller.User.FindFirstValue(ClaimTypes.GivenName);

        if (firstName is null)
        {
            throw new UnauthorizedAccessException("Missing first name claim.");
        }

        return firstName;
    }

    public static string GetUserLastName(this ControllerBase controller)
    {
        string? lastName = controller.User.FindFirstValue(ClaimTypes.Surname);

        if (lastName is null)
        {
            throw new UnauthorizedAccessException("Missing last name claim.");
        }

        return lastName;
    }
}
