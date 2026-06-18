using Common.Core.Primitives;
using EnigmaVault.PasswordService.Domain.Models;

namespace EnigmaVault.PasswordService.Application.Common.Repositories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag, CancellationToken token);
        Task<Maybe<Tag>> GetAsync(Guid id, Guid UserId, CancellationToken token = default);
        void Remove(Tag icon);
    }
}