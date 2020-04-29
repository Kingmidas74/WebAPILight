using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessServices.Services;
using BusinessServices.Models;
using MediatR;
using System;

namespace WebAPIService.MediatR
{
    public class CreateParentHandler: IRequestHandler<CreateParentCommand<Guid>, Parent<Guid>>
    {
        private readonly ParentService<Guid> parentService;
        private readonly IMapper mapper;

        public CreateParentHandler(ParentService<Guid> parentService, IMapper mapper)
        {
            this.parentService=parentService;
            this.mapper = mapper;
        }
        public async Task<Parent<Guid>> Handle(CreateParentCommand<Guid> request, CancellationToken cancellationToken)
        {
            return await this.parentService.CreateAsync(this.mapper.Map<Parent<Guid>>(request));
        }
    }
}