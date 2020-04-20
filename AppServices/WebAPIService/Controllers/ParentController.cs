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
        public PagingResult<Parent> GetAllWithPaging([FromQuery]GetAllSortPagedFilterInputParameter queryParameter)
        {
            var userId = User.ExtractIdentifier();
            return new PagingResult<Parent>() { 
                Entities = parentService.GetAllPaged(userId, queryParameter.Take, queryParameter.Skip, queryParameter.SortIndex, queryParameter.SortOrder, queryParameter.Filter), 
                TotalCount = parentService.GetAllPaged(userId, filter: queryParameter.Filter).Count };
        }

        /// <summary>
        /// Получить сущность Parent по идентификатору
        /// </summary>
        /// <param name="parentId">Идентификатор</param>
        /// <returns></returns>
        [HttpGet(nameof(GetById))]        
        public Parent GetById([FromQuery, Required]Guid parentId)
        {
            return parentService.FindOne(User.ExtractIdentifier(), parentId);
        }

        /// <summary>
        /// Удалить данные о конкретной сущности Parent
        /// </summary>
        /// <param name="irreversibleDeleteParameter"></param>
        /// <returns></returns>
        [HttpDelete(nameof(IrreversibleDelete))]
        public Guid IrreversibleDelete([FromQuery]IrreversibleDeleteInputParameter irreversibleDeleteParameter)
        {
            return parentService.Execute(new IrreversibleDeleteParameter {
                Id = irreversibleDeleteParameter.Id,
                UserId = User.ExtractIdentifier()
            });
        }
    }
}