﻿using Cel.Core.Mediator.Enums;

namespace Cel.Core.Mediator.Models;

public sealed record ValidationError : Error
{
    public ValidationError(Error[] errors)
        : base("Validation.General", "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
}