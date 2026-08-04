namespace GarageLog.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(int userId, string email, string firstName, string lastName);
}
