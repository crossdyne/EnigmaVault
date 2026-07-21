using Shared.Contracts.Messaging.Interfaces;

namespace Shared.Contracts.Messaging.Events
{
    public sealed record UserPasswordResetIntegrationEvent(Guid EventId, DateTime OccurredAtUtc, string UserId) : IIntegrationEvent;
}