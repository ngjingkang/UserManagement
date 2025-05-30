using Domain.Entities;
using Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services.Users.Commands;
using Service.Services.Users.Queries;
using WebApi.Controllers.Shared;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(ISender mediator):BaseApiController
        
    {
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest request)
        => ApiAccepted(await mediator.Send(new AddUserCommand(request)));

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
       => ApiResponse(await mediator.Send(new GetUsersQuery()));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAge(int id, [FromBody] UpdateUserRequest user)
        {
            user.Id = id;
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = "System";

            return ApiAccepted(await mediator.Send(new UpdateUserCommand(user)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await mediator.Send(new DeleteUserCommand(id));
            return result.IsSuccess ? NoContent() : NotFound(result.Errors);
        }
    }
}
