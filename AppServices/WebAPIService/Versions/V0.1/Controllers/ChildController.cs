using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIService.Versions.V01.Controllers
{

    [Route ("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ChildController : ControllerBase {
        private readonly IMediator mediator;
        public ChildController (IMediator mediator) {
            this.mediator = mediator;
        }

    }
}