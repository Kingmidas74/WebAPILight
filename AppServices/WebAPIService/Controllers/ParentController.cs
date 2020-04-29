using System;
using System.Threading.Tasks;
using BusinessServices.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIService.MediatR;

namespace WebAPIService.Controllers
{

    [Route ("api/[controller]")]
    [ApiController]
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
            return Ok(await mediator.Send(new GetAllParentsQuery<Guid>()));
        }

        /// <summary>
        /// Get parents by unique identifier
        /// </summary>
        /// <param name="Id">Unique identifier</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetParentByIdAsync(Guid Id) {
            return Ok(await mediator.Send(new GetParentByIdQuery<Guid>(Id)));
        }


        /// <summary>
        /// Create parent
        /// </summary>
        /// <param name="parent">DTO parent model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateParentAsync([FromBody]CreateParentCommand<Guid> parent) {
            var result = await mediator.Send(parent);
            return Created($"{nameof(Parent<Guid>)}/{result.Id}",result);
        }
    }
}