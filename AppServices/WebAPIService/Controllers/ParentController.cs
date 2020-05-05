using System;
using System.Threading.Tasks;
using BusinessServices.MediatR;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="parent">DTO parent model</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateParentAsync([FromBody]CreateParentCommand parent) {
            var result = await mediator.Send(parent);
            return Created($"{nameof(Parent)}/{result.Id}",result);
        }
    }
}