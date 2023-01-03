namespace CleanArchitecture.Domain.Common;

using MediatR;

public abstract class BaseEvent<T> : INotification
{
    protected BaseEvent(T item)
    {
        this.Item = item;
    }

    public T Item { get; }
}
