using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Service.Errors;
using System.Runtime.CompilerServices;

namespace WebApi.Controllers.Shared
{
    public class BaseApiController : ControllerBase
    {
        protected IActionResult ApiResponse<T>(
            Result<T> result,
            [CallerMemberName] string? caller = default)
        {
            // Handle null result
            if (result is null ||
                result.IsSuccess && result.ValueOrDefault is null)
                return HandleNullProblem();

            // Handle success result
            if (result.IsSuccess)
                return Ok(result.ValueOrDefault);

            // Handle FluentValidation errors
            if (result.Errors.Any(x => x is ValidationError))
                return HandleValidationProblem(result);

            // Handle FluentResult errros
            return HandleFluentResultsProblem(result);
        }
        protected IActionResult ApiAccepted(
            Result result,
            [CallerMemberName] string? caller = default)
        {
            // Handle success result
            if (result.IsSuccess)
                return Accepted();

            // Handle FluentValidation errors
            if (result.Errors.Any(x => x is ValidationError))
                return HandleValidationProblem(result);

            // Handle FluentResult errros
            return HandleFluentResultsProblem(result);
        }
        private IActionResult HandleNullProblem()
        {
            int statusCode = StatusCodes.Status404NotFound;

            //LogContext.PushProperty(LogConstant.StatusCode, statusCode);
            //logger.LogError(SharedResources.R90010);

            return Problem(
                statusCode: statusCode);
        }
        private IActionResult HandleValidationProblem(ResultBase result)
        {
            int statusCode = StatusCodes.Status400BadRequest;

            foreach (KeyValuePair<string, object> metadata in result.Errors.SelectMany(e => e.Metadata))
            {
                ModelState.TryAddModelError(metadata.Key, metadata.Value?.ToString() ?? $"{metadata.Key} is invalid.");
            }

            //LogContext.PushProperty(LogConstant.StatusCode, statusCode);
            //logger.LogError(JsonConvert.SerializeObject(
            //        ModelState.Values
            //        .SelectMany(ms => ms.Errors)
            //        .Select(e => e.ErrorMessage)));

            return ValidationProblem(
                detail: result?.Reasons[0]?.Message ?? "Validation Problem",
                statusCode: statusCode,
                modelStateDictionary: ModelState);
        }
        private IActionResult HandleFluentResultsProblem(ResultBase result)
        {
            var err = GetFluentResultsError(result);

            //LogContext.PushProperty(LogConstant.StatusCode, err.StatusCode);
            //logger.LogError(err.Message);

            return Problem(
                detail: err.Message,
                statusCode: err.StatusCode);
        }

        private static (int StatusCode, string Message) GetFluentResultsError(ResultBase result)
        {
            IError? firstError = result.Errors.FirstOrDefault();

            if (firstError is null)
                return (StatusCodes.Status500InternalServerError, "Fluent Result Error");

            int statusCode = firstError switch
            {
                _ => StatusCodes.Status500InternalServerError
            };

            return (statusCode, firstError.Message);
        }

    }
}
