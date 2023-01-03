namespace CleanArchitecture.Domain.Events;

using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

public class DepartmentCreatedEvent : BaseEvent<Department>
{
    public DepartmentCreatedEvent(Department item) 
        : base(item)
    {
    }
}
