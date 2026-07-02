namespace EnigmaVault.Password.Service.Application.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}