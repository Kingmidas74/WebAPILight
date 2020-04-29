using System.Threading;
using System.Threading.Tasks;
using BusinessServices.Services;
using BusinessServices.Models;
using MediatR;
using System;

namespace WebAPIService.MediatR
{
    public class GetParentByIdHandler: IRequestHandler<GetParentByIdQuery<Guid>, Parent<Guid>>
    {
        private readonly ParentService<Guid> parentService;

        public GetParentByIdHandler(ParentService<Guid> parentService)
        {
            this.parentService=parentService;
        }
        public async Task<Parent<Guid>> Handle(GetParentByIdQuery<Guid> request, CancellationToken cancellationToken)
        {
            return await this.parentService.FindOneAsync(request.Id);
        }
    }
}