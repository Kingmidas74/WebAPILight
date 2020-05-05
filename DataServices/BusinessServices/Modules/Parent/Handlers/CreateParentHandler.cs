using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessServices.Services;
using MediatR;
using Domain;

namespace BusinessServices.Modules.ParentModule
{
    public class CreateParentHandler: IRequestHandler<CreateParentCommand, Parent>
    {
        private readonly ParentService parentService;
        private readonly IMapper mapper;

        public CreateParentHandler(ParentService parentService, IMapper mapper)
        {
            this.parentService=parentService;
            this.mapper = mapper;
        }
        public async Task<Parent> Handle(CreateParentCommand request, CancellationToken cancellationToken)
        {
            return await this.parentService.CreateAsync(this.mapper.Map<Parent>(request));
        }
    }
}