using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIService.Controllers
{

    [Route ("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChildController : ControllerBase {
        private readonly IMediator mediator;
        public ChildController (IMediator mediator) {
            this.mediator = mediator;
        }

    }
}