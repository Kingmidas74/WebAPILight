using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIService.Extensions;
using MessageBusServices;
using Domain.Extensions;

namespace WebAPIService.Versions.ANY.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class StatusController : ControllerBase {
        private MessageProducerService MessageService;
        public StatusController (MessageProducerService messageService) {
            this.MessageService = messageService;
        }

        /// <summary>
        /// Открытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet (nameof (GetFreeStatus))]
        public IActionResult GetFreeStatus () {
            //MessageService.Enqueue (nameof (GetFreeStatus), MessageBusEvents.UserNotificationEvent.GetDescription ());
            return Ok (new {
                result=1
            });
        }

        /// <summary>
        /// Закрытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet (nameof (GetPrivateStatus))]
        [Authorize]
        public IActionResult GetPrivateStatus () {
            var userId = User.ExtractIdentifier ();
            //MessageService.Enqueue ($"{nameof(GetFreeStatus)} by user {userId}", MessageBusEvents.UserNotificationEvent.GetDescription () + ".test");
            return Ok (userId);
        }
    }
}