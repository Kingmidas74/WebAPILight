using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{    
    [Route ("[controller]")]
    [ApiController]
    public class IdentityController:ControllerBase
    {
        private IUserRepository UserRepository { get; }

        public IdentityController(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }   

        [HttpGet]
        public IActionResult Status() {
            return Ok();
        }

        [HttpPost(nameof(CreateIdentity))]
        public IActionResult CreateIdentity([FromBody]CreateIdentityParameter createIdentityParameter) {
            var identity = UserRepository.CreateIdentity(createIdentityParameter);
            return Ok(identity);
        }

        [HttpPost(nameof(ConfirmIdentity))]
        public IActionResult ConfirmIdentity([FromBody]ConfirmIdentityParameter confirmIdentityParameter) {
            var id = UserRepository.ConfirmIdentity(confirmIdentityParameter);
            return Redirect(confirmIdentityParameter.Redirect);
        }      
    }
}