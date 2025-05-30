using Domain.Requests;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.Extensions;
using Service.Interfaces;

namespace Service.Services.Users.Commands
{
    public sealed record AddUserCommand(AddUserRequest user) : IRequest<Result>;

    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserValidator() { }
    }

    internal sealed class AddUserCommandHandler(IUserRepository userRepository,
                                                ILogger<AddUserCommandHandler> logger,
                                                IRabbitMqService rabbitMqService)
        : IRequestHandler<AddUserCommand, Result>
    {
        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received request to add user: {@User}", request.user);

            bool result = await userRepository.AddUserAsync(request.user);
            if (result)
            {
                logger.LogInformation("User added successfully: {@User}", request.user);

                string message = $"User added: {request.user.Name}, Age: {request.user.Age}";
                await rabbitMqService.PublishAsync("user_exchange", "user_created", message);

                return Result.Ok();
            }
            else
            {
                logger.LogWarning("Failed to add user: {@User}", request.user);
                return Result.Fail("User creation failed.");
            }
        }
    }
}
