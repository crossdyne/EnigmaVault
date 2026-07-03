namespace EnigmaVault.Password.Service.Application.Features.Validators
{
    public interface IMayHasUserId
    {
        public Guid? UserId { get; }
    }
}