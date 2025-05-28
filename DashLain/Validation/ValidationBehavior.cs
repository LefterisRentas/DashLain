using DashLain.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Validation;
public sealed class ValidationBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
    where TCommand : notnull
{
    readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TCommand>> validators)
    {
        _validators = validators;
    }

    public async Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TCommand>(request);
            var failures = (await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .Select(x=> x.ErrorMessage)
                .ToArray();

            if (failures.Length != 0)
            {
                var genericType = typeof(TResult).GetGenericTypeDefinition();
                if (genericType != typeof(Result<>))
                {
                    throw new InvalidOperationException("Only 'Result<T>' return types are supported by 'ValidationBehavior'.");
                }

                var innerType = typeof(TResult).GetGenericArguments()[0];
                var failMethod = typeof(Result<>)
                    .MakeGenericType(innerType)
                    .GetMethod(nameof(Result<object>.Fail), BindingFlags.Public | BindingFlags.Static);

                return (TResult)failMethod!.Invoke(null, [failures])!;
            }
        }
        return await next();
    }
}
