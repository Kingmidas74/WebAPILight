using System.Threading;
using System.Threading.Tasks;
using BusinessServices.Services;
using MediatR;
using Domain;

namespace BusinessServices.Modules.ParentModule
{
    public class GetParentByIdHandler: IRequestHandler<GetParentByIdQuery, Parent>
    {
        private readonly ParentService parentService;

        public GetParentByIdHandler(ParentService parentService)
        {
            this.parentService=parentService;
        }
        public async Task<Parent> Handle(GetParentByIdQuery request, CancellationToken cancellationToken)
        {
            return await this.parentService.FindOneAsync(request.Id);
        }
    }
}