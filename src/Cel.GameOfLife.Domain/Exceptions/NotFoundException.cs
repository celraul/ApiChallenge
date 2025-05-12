namespace Cel.GameOfLife.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(List<string> errors)
        : base(errors)
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}