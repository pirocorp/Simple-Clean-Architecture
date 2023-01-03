namespace CleanArchitecture.Application.Departments.EventHandlers;

using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class DepartmentCreatedEventHandler 
    : INotificationHandler<DepartmentCreatedEvent>
{
    private readonly ILogger<DepartmentCreatedEventHandler> logger;

    public DepartmentCreatedEventHandler(ILogger<DepartmentCreatedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(
        DepartmentCreatedEvent notification, 
        CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Event: {Event}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
