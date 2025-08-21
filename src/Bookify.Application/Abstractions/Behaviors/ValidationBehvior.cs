using Bookify.Application.Exceptions;
using FluentValidation;
using Mediator;

namespace Bookify.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Messaging.IBaseCommand // Validation is only necessary for commands
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(TRequest request, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {

        if (!_validators.Any())
        {
            return await next(request, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
                    .Select(validator => validator.Validate(context))
                    .Where(validationResult => validationResult.Errors.Any())
                    .SelectMany(validationResult => validationResult.Errors)
                    .Select(validationFailure => new ValidationError(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage))
                    .ToList();

        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next(request, cancellationToken);
    }
}