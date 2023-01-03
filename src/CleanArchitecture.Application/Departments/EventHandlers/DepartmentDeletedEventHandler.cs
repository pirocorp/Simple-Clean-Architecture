namespace CleanArchitecture.Application.Departments.EventHandlers;

using CleanArchitecture.Domain.Events;
using MediatR;

using Microsoft.Extensions.Logging;

public class DepartmentDeletedEventHandler 
    : INotificationHandler<DepartmentDeletedEvent>
{
    private readonly ILogger<DepartmentDeletedEventHandler> logger;

    public DepartmentDeletedEventHandler(ILogger<DepartmentDeletedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(
        DepartmentDeletedEvent notification, 
        CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Event: {Event}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
