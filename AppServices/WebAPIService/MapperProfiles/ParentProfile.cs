using System;
using AutoMapper;
using WebAPIService.MediatR;

namespace WebAPIService.MapperProfiles {
    public class ParentProfile : Profile {
        public ParentProfile () {
            CreateMap<DataAccess.DataBaseEntities.Parent<Guid>, BusinessServices.Models.Parent<Guid>>().ReverseMap();                               
            CreateMap<CreateParentCommand<Guid>, BusinessServices.Models.Parent<Guid>> ();
        }
    }
}