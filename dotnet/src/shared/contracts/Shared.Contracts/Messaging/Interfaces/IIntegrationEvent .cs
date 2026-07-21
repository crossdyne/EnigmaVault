namespace Shared.Contracts.Messaging.Interfaces
{
    public interface IIntegrationEvent 
    {
        Guid EventId { get; }
        DateTime OccurredAtUtc { get; }
    }
}