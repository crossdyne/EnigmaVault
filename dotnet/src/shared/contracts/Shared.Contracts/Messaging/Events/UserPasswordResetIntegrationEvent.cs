using Shared.Contracts.Messaging.Interfaces;

namespace Shared.Contracts.Messaging.Events
{
    public sealed record UserPasswordResetIntegrationEvent(Guid IdEvent, DateTime OccurredOnUtc, Guid UserId) : IIntegrationEvent;
}