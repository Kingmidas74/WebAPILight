using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using WebAPIService.Extensions;

namespace WebAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class StatusController:ControllerBase
    {
        public StatusController()
        {
        }

        /// <summary>
        /// Открытый статус
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetFreeStatus))]
        public IActionResult GetFreeStatus()
        {
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
            return Ok(User.ExtractIdentifier());
        }
    }
}