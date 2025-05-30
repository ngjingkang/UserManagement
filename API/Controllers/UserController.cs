using API.Controllers.Shared;
using Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Users.Commands;

namespace API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(ISender mediator):BaseApiController
        
    {
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest request)
        => ApiAccepted(await mediator.Send(new AddUserCommand(request)));
            
        
    }
}
