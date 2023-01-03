namespace CleanArchitecture.Application.Common.Exceptions;

using FluentValidation.Results;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        this.Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        this.Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(params (string Property, string[] ErrorMessages)[] failures)
        : this()
    {
        this.Errors = failures.ToDictionary(
            x => x.Property,
            x => x.ErrorMessages);
    }

    public IDictionary<string, string[]> Errors { get; }
}
