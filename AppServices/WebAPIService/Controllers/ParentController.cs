using System.Collections;
using System;
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
    public class ParentController:ControllerBase
    {
        private readonly ParentService parentService;
        public ParentController(ParentService parentService)
        {
            this.parentService = parentService;
        }

        /// <summary>
        /// Получить все сущности Parent с ограничениями по паджинации и фильтрам
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetAllWithPaging))]        
        public IEnumerable GetAllWithPaging([FromQuery]GetAllSortPagedFilterInputParameter queryParameter)
        {
            var userId = User.ExtractIdentifier();
            return parentService.GetAllPaged(queryParameter.Take, queryParameter.Skip, queryParameter.SortIndex,queryParameter.SortOrder);
        }

        /// <summary>
        /// Получить сущность Parent по идентификатору
        /// </summary>
        /// <param name="parentId">Идентификатор</param>
        /// <returns></returns>
        [HttpGet(nameof(GetById))]        
        public Parent GetById([FromQuery, Required]Guid parentId)
        {
            return parentService.GetById(parentId);
        }

        /// <summary>
        /// Удалить данные о конкретной сущности Parent
        /// </summary>
        /// <param name="irreversibleDeleteParameter"></param>
        /// <returns></returns>
        [HttpDelete(nameof(IrreversibleDelete))]
        public void IrreversibleDelete([FromQuery]IrreversibleDeleteInputParameter irreversibleDeleteParameter)
        {
            parentService.RemoveById(irreversibleDeleteParameter.Id);
        }
    }
}