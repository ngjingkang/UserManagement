using Domain.Entities;
using Domain.Requests;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.Extensions;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Users.Commands;

public sealed record UpdateUserCommand(UpdateUserRequest user) : IRequest<Result>
{
    public sealed class UpdateUserAsyncValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserAsyncValidator()
        {

        }

    }
    public sealed class UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommand> logger) : IRequestHandler<UpdateUserCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received request to update user: {@User}", request.user);

            bool result = await userRepository.UpdateUserAsync(request.user);

            if (result)
            {
                logger.LogInformation("User updated successfully: {@User}", request.user);
                return Result.Ok();
            }
            else
            {
                logger.LogWarning("Failed to update user: {@User}", request.user);
                return Result.Fail("User update failed.");
            }
        }
    }

}



