using System.Threading;
using System.Threading.Tasks;
using BusinessServices.Services;
using MediatR;

namespace BusinessServices.Modules.ParentModule
{
    public class RemoveParentHandler : IRequestHandler<RemoveParentCommand>
    {

        private readonly ParentService parentService;

        public RemoveParentHandler(ParentService parentService)
        {
            this.parentService=parentService;
        }

        public async Task<Unit> Handle(RemoveParentCommand request, CancellationToken cancellationToken)
        {
            await parentService.RemoveByIdAsync(request.Id);
            return await Task.FromResult(Unit.Value);
        }
    }
}