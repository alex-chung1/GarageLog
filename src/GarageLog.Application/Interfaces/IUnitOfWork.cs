namespace GarageLog.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
