using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessServices.Services;
using MediatR;
using Domain;

namespace BusinessServices.Modules.ParentModule
{
    public class GetAllParentsHandler : IRequestHandler<GetAllParentsQuery, IEnumerable<Parent>>
    {
        private readonly ParentService parentService;

        public GetAllParentsHandler(ParentService parentService)
        {
            this.parentService=parentService;
        }
        public async Task<IEnumerable<Parent>> Handle(GetAllParentsQuery request, CancellationToken cancellationToken)
        {
            return await this.parentService.FindAllAsync();            
        }
    }
}