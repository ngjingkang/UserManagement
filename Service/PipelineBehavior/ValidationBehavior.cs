using FluentResults;
using FluentValidation;
using MediatR;
using Service.Errors;

namespace Service.PipelineBehavior
{
    public class ValidatorBehavior<TRequest, TResponse>(
       IEnumerable<IValidator<TRequest>> validators)
       : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
           where TResponse : ResultBase<TResponse>, new()
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var validationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(request, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors);

                if (failures.Any())
                {
                    var errors = failures.Select(f =>
                        new ValidationError().WithMetadata(f.PropertyName, f.ErrorMessage)
                    );

                    return new TResponse().WithErrors(errors);
                }
            }

            return await next();
        }
    }
}
