namespace Cel.GameOfLife.Domain.Exceptions;

public class ValidationException : BaseException
{
    public ValidationException(List<string> errors)
        : base(errors)
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}