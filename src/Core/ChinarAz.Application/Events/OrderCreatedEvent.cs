namespace ChinarAz.Application.Events;

public record OrderCreatedEvent(Guid OrderId, string UserEmail, decimal TotalPrice, Guid userId);
