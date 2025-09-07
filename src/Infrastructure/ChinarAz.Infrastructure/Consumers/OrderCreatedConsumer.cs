using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.Events;
using MassTransit;

namespace ChinarAz.Infrastructure.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;

    public OrderCreatedConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        if (!string.IsNullOrWhiteSpace(message.UserEmail))
        {
            await _emailService.SendEmailAsync(
                new List<string> { message.UserEmail },
                "Your Order Confirmation",
                $"✅ Your order ({message.OrderId}) has been created successfully. Total: {message.TotalPrice:C}"
            );
        }
    }
}
