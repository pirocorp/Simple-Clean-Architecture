namespace CleanArchitecture.Infrastructure.Services;

using CleanArchitecture.Application.Common.Interfaces;

internal class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
