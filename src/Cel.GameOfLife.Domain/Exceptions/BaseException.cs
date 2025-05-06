namespace Cel.GameOfLife.Domain.Exceptions;

public abstract class BaseException : Exception
{
    public List<string> Errors { get; }

    public BaseException()
        : base("One or more validation errors occurred.")
    {
        Errors = [];
    }

    public BaseException(List<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors ?? new List<string>();
    }

    public BaseException(string message)
        : base(message)
    {
        Errors = new List<string>() { message };
    }

    public BaseException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new List<string>() { message };
    }
}