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

namespace Service.Services.Users.Commands
{
    public sealed record DeleteUserCommand(int id): IRequest<Result>
    {
        public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
        {
            public DeleteUserValidator()
            {
               
            }
        }

        internal sealed class DeleteUserCommandHandler(IUserRepository userRepository, ILogger<DeleteUserCommand> logger) : IRequestHandler<DeleteUserCommand, Result>
        {
            public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("Received request to delete user: {@User}", request.id);

                bool result = await userRepository.DeleteUserAsync(request.id);
                if (result)
                {
                    logger.LogInformation("User deleted successfully: {@User}", request.id);
                    return Result.Ok();
                }
                else
                {
                    logger.LogWarning("Failed to delete user: {@User}", request.id);
                    return Result.Fail("User deletion failed.");
                }
            }
        }
    }
}
