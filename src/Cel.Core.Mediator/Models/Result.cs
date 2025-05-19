using System.Diagnostics.CodeAnalysis;

namespace Cel.Core.Mediator.Models;

public class Result
{
    public Result(bool isSuccess, List<Error> error)
    {
        IsSuccess = isSuccess;
        Errors = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public List<Error> Errors { get; }

    public static Result Success() => new(true, []);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, []);

    public static Result Failure(List<Error> errors) => new(false, errors);

    public static Result<TValue> Failure<TValue>(List<Error> errors) =>
        new(default, false, errors);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, List<Error> errors)
        : base(isSuccess, errors)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>([]);

    public static Result<TValue> ValidationFailure(List<Error> errors) =>
        new(default, false, errors);
}

