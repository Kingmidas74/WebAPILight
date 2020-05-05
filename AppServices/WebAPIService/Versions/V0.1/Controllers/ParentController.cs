using System;
using System.Threading.Tasks;
using BusinessServices.Modules.ParentModule;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIService.Versions.V01.Controllers
{

    [Route ("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("0.1")]
    [Authorize]
    public class ParentController : ControllerBase {
        private readonly IMediator mediator;
        public ParentController (IMediator mediator) {            
            this.mediator = mediator;
        }


        /// <summary>
        /// Get parents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllParentsAsync() {
            return Ok(await mediator.Send(new GetAllParentsQuery()));
        }

        /// <summary>
        /// Get parents by unique identifier
        /// </summary>
        /// <param name="Id">Unique identifier</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetParentByIdAsync(Guid Id) {
            return Ok(await mediator.Send(new GetParentByIdQuery(Id)));
        }


        /// <summary>
        /// Create parent
        /// </summary>
        /// <param name="createParentCommang">DTO parent model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateParentAsync([FromBody]CreateParentCommand createParentCommang) {
            var result = await mediator.Send(createParentCommang);
            return Created($"{nameof(Parent)}/{result.Id}",result);
        }

        /// <summary>
        /// Remove parent
        /// </summary>
        /// <param name="removeParentCommand">DTO parent model</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveParentAsync([FromBody]RemoveParentCommand removeParentCommand) {
            await mediator.Send(removeParentCommand);
            return Ok();
        }
    }
}