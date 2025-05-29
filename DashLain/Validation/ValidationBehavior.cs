using DashLain.Models;
using FluentValidation;
using MediatR;
using System.Reflection;

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
                if (genericType != typeof(UIResult<>))
                {
                    throw new InvalidOperationException("Only 'UIResult<T>' return types are supported by 'ValidationBehavior'.");
                }

                var innerType = typeof(TResult).GetGenericArguments()[0];
                var failMethod = typeof(UIResult)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == nameof(UIResult.Fail)
                        && m.IsGenericMethodDefinition
                        && m.GetGenericArguments().Length == 1
                        && m.GetParameters().Length == 1
                        && m.GetParameters()[0].ParameterType == typeof(IEnumerable<string>));

                var genericFail = failMethod.MakeGenericMethod(innerType);

                return (TResult)genericFail.Invoke(null, [failures])!;
            }
        }
        return await next();
    }
}
