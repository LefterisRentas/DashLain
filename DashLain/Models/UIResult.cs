using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Models;

public class UIResult<T> {
    public bool IsSuccess { get; }

    public T? Value { get; }

    public List<string> Errors { get; } = [];

    internal UIResult(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    internal UIResult(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors.AddRange(errors);
    }

    public override string ToString() => IsSuccess ? $"Success: {Value}" : $"Failure: {string.Join(", ", Errors)}";
}

public static class UIResult {
    public static UIResult<T> Succeed<T>(T value) => new(value);

    public static UIResult<T> Fail<T>(params string[] errors) => new(errors);

    public static UIResult<T> Fail<T>(IEnumerable<string> errors) => new(errors);
}
