using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessServices.Extensions;
using BusinessServices.Services;
using Contracts.Shared.Parameters;
using Contracts.Shared.Results;
using Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIService.Extensions;
using WebAPIService.Models;

namespace WebAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChildController:ControllerBase
    {
        private readonly ChildService childService;
        public ChildController(ChildService childService)
        {
            this.childService = childService;
        }

        /// <summary>
        /// Получить всех детей родителя
        /// </summary>
        /// <param name="getAllByParent"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetAllByParent))]
        public IEnumerable<Child> GetAllByParent([FromQuery]GetAllByParentInputParameter getAllByParent)
        {
            return childService.GetAllByParent(User.ExtractIdentifier(),getAllByParent.ParentId);
        }
    }
}