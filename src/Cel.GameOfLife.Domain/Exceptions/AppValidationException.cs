namespace Cel.GameOfLife.Domain.Exceptions;

public class AppValidationException : BaseException
{
    public AppValidationException(List<string> errors)
        : base(errors)
    {
    }

    public AppValidationException(string message)
        : base(message)
    {
    }

    public AppValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}