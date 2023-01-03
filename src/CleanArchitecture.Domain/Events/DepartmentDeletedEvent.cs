namespace CleanArchitecture.Domain.Events;

using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

public class DepartmentDeletedEvent : BaseEvent<Department>
{
    public DepartmentDeletedEvent(Department item) 
        : base(item)
    {
    }
}
