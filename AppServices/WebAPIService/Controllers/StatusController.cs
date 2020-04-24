using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using WebAPIService.Extensions;
using WebAPIService.Services;

namespace WebAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class StatusController:ControllerBase
    {
        private MessageService MessageService;
        public StatusController(MessageService messageService)
        {
            this.MessageService = messageService;
        }

        /// <summary>
        /// Открытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetFreeStatus))]
        public IActionResult GetFreeStatus()
        {
            MessageService.Enqueue(nameof(GetFreeStatus),"public");
            return Ok();
        }

        /// <summary>
        /// Закрытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetPrivateStatus))]
        [Authorize]
        public IActionResult GetPrivateStatus()
        {
            var userId = User.ExtractIdentifier();
            MessageService.Enqueue($"{nameof(GetFreeStatus)} by user {userId}","private");
            return Ok(userId);
        }
    }
}