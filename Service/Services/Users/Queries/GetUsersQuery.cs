using Domain.Entities;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.Extensions;
using Service.Interfaces;
using Service.Services.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Users.Queries
{
    public sealed record GetUsersQuery : IRequest<Result<IEnumerable<User>>>;

    internal sealed class GetUsersQueryHandler(IUserRepository userRepository, ILogger<GetUsersQuery> logger) : IRequestHandler<GetUsersQuery, Result<IEnumerable<User>>>
    {

        public async Task<Result<IEnumerable<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieve request to get uses list");

            var result = await userRepository.GetUsersAsync();
            if (result.Count() > 0)
            {
                logger.LogInformation("User list retrieved successfully");
                return Result.Ok();
            }
            else
            {
                logger.LogWarning("Failed to retrieve user list");
                return Result.Fail("User list retrival failed.");
            }
        }
    }

}



