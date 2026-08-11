using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.AddTag;
using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Common;
using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;
using EnigmaVault.Password.Service.Infrastructure.Persistence;
using EnigmaVault.Password.Service.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnigmaVault.Password.Service.Integration.Tests.Handlers.Commands
{
    public class AddTagToVaultItemCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly AddTagToVaultItemCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public AddTagToVaultItemCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var vaultRepo = new VaultItemRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new AddTagToVaultItemCommandHandler(vaultRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_AttachTagToVaultItem()
        {
            var userId = UserId.Create(Guid.NewGuid());
            var tagName = TagName.Create("ValidName");
            var color = Color.FromHex("#F1F3F9");
            var tag = Tag.Create(userId, tagName, color);

            var vault = VaultItem.Create(userId, VaultType.Create(1), IconId.Create(Guid.NewGuid()), EncryptedData.Create("encryptedOverview"), EncryptedData.Create("encryptedDetails"), CryptoVersion.Create(1));

            await _fixture.DbContext.Set<Tag>().AddAsync(tag, _cancellationToken);
            await _fixture.DbContext.Set<VaultItem>().AddAsync(vault, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new AddTagToVaultItemCommand(userId, vault.Id, tag.Id);
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vault.Id, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            vaultInDb.Should().NotBeNull();
            vaultInDb.Tags.Should().HaveCount(1);
            vaultInDb.Tags.FirstOrDefault()?.TagId.Should().Be(tag.Id);
        }

        [Fact]
        public async Task Handle_ValidCommand_NotFoundVaultItemReturnNotFoundError()
        {
            var vaultItemId = VaultItemId.Create(Guid.NewGuid());
            var command = new AddTagToVaultItemCommand(Guid.NewGuid(), vaultItemId, Guid.NewGuid());
            Result<Unit> result = await _handler.Handle(command, _cancellationToken);

            var vaultInDb = await _fixture.DbContext.Set<VaultItem>().Include(v => v.Tags).FirstOrDefaultAsync(v => v.Id == vaultItemId, _cancellationToken);

            result.IsFailure.Should().BeTrue();
            result.Errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound)?.Code.Should().Be(ErrorCode.NotFound);
            vaultInDb.Should().BeNull();
        }
    }
}