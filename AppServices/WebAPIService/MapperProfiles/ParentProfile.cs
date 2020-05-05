using AutoMapper;
using BusinessServices.MediatR;
using Domain;

namespace WebAPIService.MapperProfiles {
    public class ParentProfile : Profile {
        public ParentProfile () {
            CreateMap<CreateParentCommand, Parent> ();
        }
    }
}