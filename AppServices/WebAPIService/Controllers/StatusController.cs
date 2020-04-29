using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIService.Enums;
using WebAPIService.Extensions;
using WebAPIService.Services;

namespace WebAPIService.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase {
        private MessageService MessageService;
        public StatusController (MessageService messageService) {
            this.MessageService = messageService;
        }

        /// <summary>
        /// Открытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet (nameof (GetFreeStatus))]
        public IActionResult GetFreeStatus () {
            MessageService.Enqueue (nameof (GetFreeStatus), MessageBusEvents.UserNotificationEvent.GetDescription ());
            return Ok ();
        }

        /// <summary>
        /// Закрытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet (nameof (GetPrivateStatus))]
        [Authorize]
        public IActionResult GetPrivateStatus () {
            var userId = User.ExtractIdentifier ();
            MessageService.Enqueue ($"{nameof(GetFreeStatus)} by user {userId}", MessageBusEvents.UserNotificationEvent.GetDescription () + ".test");
            return Ok (userId);
        }
    }
}