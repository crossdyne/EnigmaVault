using Crossdyne.Toolkit.Primitives;
using EnigmaVault.Password.Service.Domain.Models;

namespace EnigmaVault.Password.Service.Application.Common.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag, CancellationToken token);
        Task<Maybe<Tag>> GetAsync(Guid id, Guid UserId, CancellationToken token = default);
        void Remove(Tag icon);
        Task<int> RemoveAllAsync(Domain.ValueObjects.User.UserId userId);
    }
}