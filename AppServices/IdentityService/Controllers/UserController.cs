using System.Threading.Tasks;
using IdentityService.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{    
    [Route ("[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class UserController:ControllerBase
    {
        private readonly IMediator Mediator;

        public UserController(IMediator mediator)
        {
            this.Mediator = mediator;
        }   

        /// <summary>
        /// Check status of IS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Status() {
            return Ok();
        }

        /// <summary>
        /// SelfRemove user 
        /// </summary>
        /// <param name="removeUserCommand"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody]RemoveUserCommand removeUserCommand) {
            await this.Mediator.Send(removeUserCommand);
            return Ok();
        }     
    }
}