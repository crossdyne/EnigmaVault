namespace EnigmaVault.Secret.Service.Application.Features.Validators
{
    public interface IMayHasUserId
    {
        public Guid? UserId { get; }
    }
}