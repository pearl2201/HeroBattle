using FluentValidation.Results;

namespace MasterServer.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(string property, string errorMessage) : this()
    {
        Errors = new Dictionary<string, string[]>() { { property, new[] { errorMessage } } };
    }

    public ValidationException(IDictionary<string, string[]> errors) : this()
    {
        Errors = errors;
    }

    public ValidationException(string property, string[] errorMessages) : this()
    {
        Errors = new Dictionary<string, string[]>() { { property, errorMessages } };
    }

    public IDictionary<string, string[]> Errors { get; }
}
