using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashLain.Models;

public class Result<T> {
    public bool IsSuccess { get; }

    public T? Value { get; }

    public List<string> Errors { get; } = [];

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors.AddRange(errors);
    }

    public static Result<T> Succeed(T value) => new(value);

    public static Result<T> Fail(IEnumerable<string> errors) => new(errors);

    public override string ToString() => IsSuccess ? $"Success: {Value}" : $"Failure: {string.Join(", ", Errors)}";
}

