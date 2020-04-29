using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessServices.Services;
using BusinessServices.Models;
using MediatR;
using System;

namespace WebAPIService.MediatR
{
    public class GetAllParentsHandler : IRequestHandler<GetAllParentsQuery<Guid>, IEnumerable<Parent<Guid>>>
    {
        private readonly ParentService<Guid> parentService;

        public GetAllParentsHandler(ParentService<Guid> parentService)
        {
            this.parentService=parentService;
        }
        public async Task<IEnumerable<Parent<Guid>>> Handle(GetAllParentsQuery<Guid> request, CancellationToken cancellationToken)
        {
            return await this.parentService.FindAllAsync();
        }
    }
}